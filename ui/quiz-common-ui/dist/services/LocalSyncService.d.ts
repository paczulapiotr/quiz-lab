import { SyncReceiveCallback, SyncReceiveDefinitionNames, SyncSendData, SyncSendDefinitionNames } from './types';
export declare class LocalSyncService {
    private wsUrl;
    private hubConnection?;
    private interval;
    private onConnectedCallbacks;
    private onDisconnectedCallbacks;
    private callbacks;
    constructor(wsUrl: string, onConnected?: () => void, onDisconnected?: () => void);
    addOnConnectedCallback: (cb: () => void) => void;
    addOnDisconnectedCallback: (cb: () => void) => void;
    removeOnConnectedCallback: (cb: () => void) => void;
    removeOnDisconnectedCallback: (cb: () => void) => void;
    onSync: <T extends SyncReceiveDefinitionNames>(definitionName: T, callback: SyncReceiveCallback<T>, key: string) => void;
    offSync: (definitionName: SyncReceiveDefinitionNames, key?: string) => void;
    private reloadSync;
    sendSync: <T extends SyncSendDefinitionNames>(definitionName: T, data?: SyncSendData[T]) => Promise<void>;
    private _connect;
    init(): Promise<void>;
    _reconnectInterval: () => void;
    isConnected(): boolean;
    dispose(): Promise<void>;
}
