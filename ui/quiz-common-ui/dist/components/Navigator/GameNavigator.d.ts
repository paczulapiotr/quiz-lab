import { GameStatus } from 'quiz-common-ui';
type Props = {
    basePath: string;
    pages: {
        [GameStatus.GameCreated]: JSX.Element;
        [GameStatus.GameJoined]: JSX.Element;
        [GameStatus.GameStarting]: JSX.Element;
        [GameStatus.RulesExplaining]: JSX.Element;
        [GameStatus.MiniGameStarting]: JSX.Element;
        [GameStatus.MiniGameStarted]: JSX.Element;
        [GameStatus.MiniGameEnding]: JSX.Element;
        [GameStatus.GameEnding]: JSX.Element;
    };
};
declare const GameNavigator: ({ pages, basePath }: Props) => import("react/jsx-runtime").JSX.Element;
export default GameNavigator;
