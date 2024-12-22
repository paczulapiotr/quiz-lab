import { default as React } from 'react';
type Props = {
    startSeconds: number;
    onTimeUp?: () => void;
};
declare const Timer: React.FC<Props>;
export default Timer;
