import * as signalR from "@microsoft/signalr";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
  SyncSendData,
  SyncSendDefinitionNames,
} from "./types";

export class LocalSyncService {
  private hubConnection?: signalR.HubConnection;
  private interval: NodeJS.Timeout | null = null;
  private onConnectedCallbacks: (() => void)[] = [];
  private onDisconnectedCallbacks: (() => void)[] = [];

  constructor(
    private wsUrl: string,
    onConnected?: () => void,
    onDisconnected?: () => void,
  ) {
    if (onConnected) this.onConnectedCallbacks.push(onConnected);
    if (onDisconnected) this.onDisconnectedCallbacks.push(onDisconnected);

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.wsUrl)
      //   .withAutomaticReconnect()
      .build();
  }

  addOnConnectedCallback = (cb: () => void) => {
    this.onConnectedCallbacks.push(cb);
  };

  addOnDisconnectedCallback = (cb: () => void) => {
    this.onDisconnectedCallbacks.push(cb);
  };

  removeOnConnectedCallback = (cb: () => void) => {
    this.onConnectedCallbacks = this.onConnectedCallbacks.filter(
      (callback) => callback !== cb,
    );
  };

  removeOnDisconnectedCallback = (cb: () => void) => {
    this.onDisconnectedCallbacks = this.onDisconnectedCallbacks.filter(
      (callback) => callback !== cb,
    );
  };

  onSync = <T extends SyncReceiveDefinitionNames>(
    definitionName: T,
    callback: SyncReceiveCallback<T>,
  ) => {
    this.hubConnection?.on(definitionName, (dto) => {
      console.debug(`Received ${definitionName}:`, dto);
      callback(dto);
    });
  };

  offSync = (definitionName: SyncReceiveDefinitionNames) => {
    this.hubConnection?.off(definitionName);
  };

  sendSync = async <T extends SyncSendDefinitionNames>(
    definitionName: T,
    data?: SyncSendData[T],
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
        `LocalSyncService is already connected to ${this.hubConnection?.baseUrl}`,
      );
      return;
    }

    this.removeOnDisconnectedCallback(this._reconnectInterval);
    this.addOnDisconnectedCallback(this._reconnectInterval);

    await this._connect();
  }

  _reconnectInterval = () => {
    console.log("TIMEOUT INITIALIZED");

    this.interval = setTimeout(async () => {
      console.log("TIMEOUT RUNNING");
      if (
        this.hubConnection?.state === signalR.HubConnectionState.Disconnected
      ) {
        await this._connect();
      }
    }, 1000);
  };

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
