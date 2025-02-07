import { useEffect, useRef } from "react";

type Props = {
  src: string;
  play: boolean;
  onFinished?: () => void;
};

const AudioPlayer = ({ src, play, onFinished }: Props) => {
  const audioRef = useRef<HTMLAudioElement | null>(null);

  useEffect(() => {
    if (play) {
      audioRef.current?.play();
    } else {
      audioRef.current?.pause();
    }
  }, [play]);

  useEffect(() => {
    const handleEnded = () => {
      if (onFinished) {
        onFinished();
      }
    };

    const audioElement = audioRef.current;
    audioElement?.addEventListener("ended", handleEnded);

    return () => {
      audioElement?.removeEventListener("ended", handleEnded);
    };
  }, [onFinished]);

  return <audio ref={audioRef} src={src} />;
};

export default AudioPlayer;
