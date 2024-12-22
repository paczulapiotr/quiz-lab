import { SyncReceiveDefinitionNames } from '../../services/types';
interface Props<TMessage> {
    basePath?: string;
    identifier: string;
    routes: Record<string, JSX.Element>;
    queueName: SyncReceiveDefinitionNames;
    createNavigationPath: (message: TMessage) => string;
}
declare const GenericNavigator: <TMessage>({ queueName, basePath, identifier: key, createNavigationPath, routes, }: Props<TMessage>) => import("react/jsx-runtime").JSX.Element;
export default GenericNavigator;
