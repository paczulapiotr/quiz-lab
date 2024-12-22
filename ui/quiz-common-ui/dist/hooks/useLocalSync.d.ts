export declare const useLocalSync: () => {
    sendSync: <T extends import('../services/types').SyncSendDefinitionNames>(definitionName: T, data?: import('../services/types').SyncSendData[T]) => Promise<void>;
};
