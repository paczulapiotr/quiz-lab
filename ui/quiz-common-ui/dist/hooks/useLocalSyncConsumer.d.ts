import { SyncReceiveCallback, SyncReceiveDefinitionNames } from '../services/types';
export declare const useLocalSyncConsumer: <T extends SyncReceiveDefinitionNames>(name: T, key: string, callback: SyncReceiveCallback<T>) => void;
