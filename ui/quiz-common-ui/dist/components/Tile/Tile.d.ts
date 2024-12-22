type Props = {
    text: string;
    blue?: boolean;
    className?: string;
    selected?: boolean;
    success?: boolean;
    failure?: boolean;
    onClick?: () => void;
};
declare const Tile: ({ text, blue, selected, success, failure, onClick, }: Props) => import("react/jsx-runtime").JSX.Element;
export default Tile;
