import React, { useRef, useEffect, useCallback } from "react";
import MyWorker from "./erasedPercentageWorker?worker&inline";
import throttle from "lodash/throttle";

type EraserCanvasProps = {
  className?: string;
  /** The overall canvas width. */
  canvasWidth?: number;
  /** The overall canvas height. */
  canvasHeight?: number;
  /** Thickness of the eraser stroke (in pixels). */
  eraserSize?: number;
  /** Invoked when a specified percentage of the square area is erased. */
  onCleared?: () => void;
  /** URL of the background image to be erased. */
  backgroundImageUrl?: string;
  /** Percentage of the image that needs to be erased to invoke onCleared. */
  clearPercentage?: number;
  /** Duration of the spreading effect in milliseconds. */
  animationTime?: number;
  /** Distance the erasing effect spreads in pixels. */
  spreadSize?: number;
};

const EraserCanvas: React.FC<EraserCanvasProps> = ({
  className,
  canvasWidth = 800,
  canvasHeight = 600,
  eraserSize = 10,
  onCleared,
  backgroundImageUrl,
  clearPercentage = 90,
  animationTime = 2000,
  spreadSize = 10,
}) => {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const lastPos = useRef<{ x: number; y: number } | null>(null);
  const cleared = useRef(false);
  const erasingQueue = useRef<{ x: number; y: number; timestamp: number }[]>(
    [],
  );
  const workerRef = useRef<Worker | null>(null);

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const checkErasedPercentage = useCallback(
    throttle((ctx: CanvasRenderingContext2D) => {
      if (!cleared.current && onCleared) {
        const imageData = ctx.getImageData(
          0,
          0,
          canvasRef.current!.width,
          canvasRef.current!.height,
        );
        const totalPixels = imageData.width * imageData.height;
        workerRef.current?.postMessage({
          imageData,
          totalPixels,
          clearPercentage,
        });
      }
    }, 500),
    [onCleared, clearPercentage],
  );

  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const ctx = canvas.getContext("2d", { willReadFrequently: true });
    if (!ctx) return;

    const image = new Image();
    if (backgroundImageUrl) {
      image.src = backgroundImageUrl;
      image.onload = () => {
        ctx.drawImage(image, 0, 0, canvasWidth, canvasHeight);
      };
    } else {
      ctx.fillStyle = "#ff0000";
      ctx.fillRect(0, 0, canvasWidth, canvasHeight);
    }

    const handleMouseMove = (event: MouseEvent) => {
      const rect = canvas.getBoundingClientRect();
      const x = event.clientX - rect.left;
      const y = event.clientY - rect.top;
      ctx.globalCompositeOperation = "destination-out";
      ctx.beginPath();
      if (lastPos.current) {
        ctx.moveTo(lastPos.current.x, lastPos.current.y);
        ctx.lineTo(x, y);
        ctx.lineWidth = eraserSize * 2;
        ctx.stroke();
      }
      ctx.arc(x, y, eraserSize, 0, Math.PI * 2);
      ctx.fill();
      lastPos.current = { x, y };

      // Add the current position to the erasing queue
      erasingQueue.current.push({ x, y, timestamp: Date.now() });

      // Throttle checking of the erased percentage
      checkErasedPercentage(ctx);
    };

    const handleMouseLeave = () => {
      lastPos.current = null;
    };

    const spreadErasingEffect = () => {
      const now = Date.now();
      erasingQueue.current = erasingQueue.current.filter(
        ({ x, y, timestamp }, index, array) => {
          const elapsed = now - timestamp;
          if (elapsed < animationTime) {
            const spreadDistance = (elapsed / animationTime) * spreadSize;
            ctx.globalCompositeOperation = "destination-out";
            ctx.beginPath();
            if (index > 0) {
              const prev = array[index - 1];
              ctx.moveTo(prev.x, prev.y);
              ctx.lineTo(x, y);
              ctx.lineWidth = eraserSize * 2 + spreadDistance * 2;
              ctx.stroke();
            }
            ctx.arc(x, y, eraserSize + spreadDistance, 0, Math.PI * 2);
            ctx.fill();
            return true;
          }
          return false;
        },
      );
      requestAnimationFrame(spreadErasingEffect);
    };

    canvas.addEventListener("mousemove", handleMouseMove);
    canvas.addEventListener("mouseleave", handleMouseLeave);

    requestAnimationFrame(spreadErasingEffect);

    return () => {
      canvas.removeEventListener("mousemove", handleMouseMove);
      canvas.removeEventListener("mouseleave", handleMouseLeave);
    };
  }, [
    backgroundImageUrl,
    eraserSize,
    onCleared,
    clearPercentage,
    animationTime,
    spreadSize,
    checkErasedPercentage,
    canvasWidth,
    canvasHeight,
  ]);

  useEffect(() => {
    const worker = new MyWorker();
    workerRef.current = worker;

    worker.onmessage = (e) => {
      if (e.data && onCleared) {
        cleared.current = true;
        onCleared();
      }
    };

    return () => {
      worker.terminate();
    };
  }, [onCleared]);

  return (
    <canvas
      className={className}
      ref={canvasRef}
      width={canvasWidth}
      height={canvasHeight}
    />
  );
};

export default EraserCanvas;
