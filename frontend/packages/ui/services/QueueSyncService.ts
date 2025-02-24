import * as signalR from "@microsoft/signalr";
import {
  SyncReceiveCallback,
  SyncReceiveData,
  SyncReceiveDefinitionNames,
  SyncSendData,
  SyncSendDefinitionNames,
} from "./types";

export class QueueSyncService {
  private messagesQueue: Record<
    SyncReceiveDefinitionNames,
    SyncReceiveData[SyncReceiveDefinitionNames][]
  > = {
    GameStatusUpdate: [],
    MiniGameNotification: [],
    Pong: [],
    SelectAnswer: [],
  };

  private messageCallbacks: Record<
    SyncReceiveDefinitionNames,
    SyncReceiveCallback<SyncReceiveDefinitionNames>[]
  > = {
    GameStatusUpdate: [],
    MiniGameNotification: [],
    Pong: [],
    SelectAnswer: [],
  };

  private silentCallbacks: Record<
    SyncReceiveDefinitionNames,
    SyncReceiveCallback<SyncReceiveDefinitionNames>[]
  > = {
    GameStatusUpdate: [],
    MiniGameNotification: [],
    Pong: [],
    SelectAnswer: [],
  };

  private hubConnection?: signalR.HubConnection;
  private interval: NodeJS.Timeout | null = null;
  private onConnectedCallbacks: (() => void)[] = [];
  private onDisconnectedCallbacks: (() => void)[] = [];
  private messageReceivedIntervals: Record<
    SyncReceiveDefinitionNames,
    NodeJS.Timeout | null
  > = {
    GameStatusUpdate: null,
    MiniGameNotification: null,
    Pong: null,
    SelectAnswer: null,
  };

  constructor(
    private wsUrl: string,
    private messageQueues: SyncReceiveDefinitionNames[],
    onConnected?: () => void,
    onDisconnected?: () => void
  ) {
    if (onConnected) this.onConnectedCallbacks.push(onConnected);
    if (onDisconnected) this.onDisconnectedCallbacks.push(onDisconnected);

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.wsUrl)
      .withKeepAliveInterval(20_000)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: () => 5000,
      })
      .build();

    this.messageQueues.forEach((queue) => {
      this.hubConnection?.on(queue, (dto) => {
        this.messagesQueue[queue].push(dto);
        if (this.messageReceivedIntervals[queue] != null) {
          clearInterval(this.messageReceivedIntervals[queue]);
          this.messageReceivedIntervals[queue] = null;
        }
        this.onMessageReceived(queue);
      });
    });
  }

  private onMessageReceived = (queue: SyncReceiveDefinitionNames) => {
    if (this.silentCallbacks[queue].length > 0) {
      this.messagesQueue[queue].forEach((mess) => {
        this.silentCallbacks[queue].forEach((cb) => cb(mess));
      });
    }

    if (this.messageCallbacks[queue].length > 0) {
      this.messagesQueue[queue].forEach((mess) => {
        this.messageCallbacks[queue].forEach((cb) => cb(mess));
      });
      this.messagesQueue[queue] = [];
    }
  };

  addOnConnectedCallback = (cb: () => void) => {
    this.onConnectedCallbacks.push(cb);
  };

  addOnDisconnectedCallback = (cb: () => void) => {
    this.onDisconnectedCallbacks.push(cb);
  };

  removeOnConnectedCallback = (cb: () => void) => {
    this.onConnectedCallbacks = this.onConnectedCallbacks.filter(
      (callback) => callback !== cb
    );
  };

  removeOnDisconnectedCallback = (cb: () => void) => {
    this.onDisconnectedCallbacks = this.onDisconnectedCallbacks.filter(
      (callback) => callback !== cb
    );
  };

  onSync = <T extends SyncReceiveDefinitionNames>(
    definitionName: T,
    callbacks: SyncReceiveCallback<T>[],
    silentCallbacks: SyncReceiveCallback<T>[]
  ) => {
    (this.messageCallbacks[definitionName] as SyncReceiveCallback<T>[]) =
      callbacks ?? [];
    (this.silentCallbacks[definitionName] as SyncReceiveCallback<T>[]) =
      silentCallbacks ?? [];

    // ensure all callbacks are registered
    if (this.messageReceivedIntervals[definitionName] != null) {
      clearInterval(this.messageReceivedIntervals[definitionName]);
    }

    this.messageReceivedIntervals[definitionName] = setTimeout(() => {
      this.onMessageReceived(definitionName);
    }, 150);
  };

  sendSync = async <T extends SyncSendDefinitionNames>(
    definitionName: T,
    data?: SyncSendData[T]
  ) => {
    if (data != null) {
      await this.hubConnection?.invoke(definitionName, data);
    } else {
      await this.hubConnection?.invoke(definitionName);
    }
  };

  private async _connect() {
    try {
      this.hubConnection?.onclose(() => {
        this.onDisconnectedCallbacks.forEach((cb) => cb());
      });

      await this.hubConnection?.start();

      this.onConnectedCallbacks.forEach((cb) => cb());
    } catch (err) {
      console.error("Error while starting connection: ", err);
      this.onDisconnectedCallbacks.forEach((cb) => cb());
    }
  }

  async init() {
    if (this.isConnected()) {
      console.warn(
        `LocalSyncService is already connected to ${this.hubConnection?.baseUrl}`
      );
      return;
    }

    await this._connect();
  }

  isConnected() {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }

  async dispose() {
    if (this.interval) {
      clearInterval(this.interval);
      this.interval = null;
    }

    if (this.hubConnection) {
      try {
        await this.hubConnection.stop();
      } catch (err) {
        console.error("Error while stopping connection: ", err);
      }
    }
  }
}
