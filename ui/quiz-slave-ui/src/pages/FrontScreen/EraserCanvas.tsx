import { FC, useEffect, useRef } from "react";
import {
  Application,
  Graphics,
  BLEND_MODES,
  LINE_CAP,
  LINE_JOIN,
} from "pixi.js";

type EraserCanvasProps = {
  /** The overall canvas width. */
  canvasWidth?: number;
  /** The overall canvas height. */
  canvasHeight?: number;
  /** Size of the red square background to erase. */
  squareSize?: number;
  /** Thickness of the eraser stroke (in pixels). */
  eraserSize?: number;
  /** Invoked when ≥90% of the square area is erased. */
  onCleared?: () => void;
};

const EraserCanvas: FC<EraserCanvasProps> = ({
  canvasWidth = 600,
  canvasHeight = 400,
  squareSize = 300,
  eraserSize = 30,
  onCleared,
}) => {
  const containerRef = useRef<HTMLDivElement | null>(null);
  const isClearedRef = useRef(false); // Prevent multiple `onCleared` calls

  useEffect(() => {
    // 1) Create the Pixi Application
    const app = new Application({
      width: canvasWidth,
      height: canvasHeight,
      // backgroundAlpha: 0, // Transparent behind
      backgroundColor: "0x000000",
      antialias: true,
    });

    if (!containerRef.current) return;
    containerRef.current.appendChild(app.view as HTMLCanvasElement);

    // 2) Draw the red square that we'll be erasing
    //    - We'll draw it in the top-left corner of the canvas
    //    - If you need it centered, you can offset (x,y) when drawing the rect
    const background = new Graphics();
    background.beginFill(0xff0000);
    background.drawRect(0, 0, squareSize, squareSize);
    background.endFill();
    app.stage.addChild(background);

    // Make the stage interactive so we can track pointer events
    app.stage.interactive = true;
    app.stage.hitArea = background.getLocalBounds();

    // Erasing state
    let isErasing = false;
    let lastPos: { x: number; y: number } | null = null;

    /**
     * Check how much of the square is erased (≥90% => onCleared).
     * We do this by rendering the background into a texture,
     * then reading the pixel data and counting how many are fully transparent.
     */
    const checkErasedArea = async () => {
      if (!onCleared || isClearedRef.current) return;

      // Render the 'background' object into a texture
      const texture = app.renderer.generateTexture(background);

      // Extract into a canvas
      const eraseCanvas = app.renderer.extract.canvas(texture);
      const ctx = eraseCanvas.getContext("2d");
      if (!ctx) return;

      const { width: w, height: h } = eraseCanvas;

      // We only care about the portion of the canvas that's actually the red square
      // so let's clamp the area to `squareSize` if it's smaller than the texture size
      const clampW = Math.min(w, squareSize);
      const clampH = Math.min(h, squareSize);

      // Get image data for the relevant area
      const imageData = ctx.getImageData(0, 0, clampW, clampH).data;

      let transparentCount = 0;
      for (let i = 3; i < imageData.length; i += 4) {
        if (imageData[i] === 0) {
          transparentCount++;
        }
      }

      const totalPixels = clampW * clampH;
      const fractionErased = transparentCount / totalPixels;

      if (fractionErased >= 0.9) {
        isClearedRef.current = true;
        onCleared();
      }
    };

    // 3) Pointer events
    app.stage.on("pointerdown", (event) => {
      isErasing = true;
      const { x, y } = event.data.global;
      lastPos = { x, y };
    });

    app.stage.on("pointerup", () => {
      isErasing = false;
      lastPos = null;
    });

    app.stage.on("pointermove", async (event) => {
      if (!isErasing || !lastPos) return;

      const { x, y } = event.data.global;

      // 4) Draw a line from the previous pointer pos to the current
      const eraserLine = new Graphics();
      eraserLine.lineStyle({
        width: eraserSize,
        color: 0xffffff,
        alpha: 1,
        cap: LINE_CAP.ROUND,
        join: LINE_JOIN.ROUND,
      });
      eraserLine.blendMode = BLEND_MODES.ERASE;
      eraserLine.moveTo(lastPos.x, lastPos.y);
      eraserLine.lineTo(x, y);

      // Add it to the background for erasing
      background.addChild(eraserLine);

      // Update position
      lastPos = { x, y };

      // 5) Check how much is erased (in a real app, you might throttle this)
      await checkErasedArea();
    });

    // Cleanup on unmount
    return () => {
      app.destroy(true, true);
    };
  }, [canvasWidth, canvasHeight, squareSize, eraserSize, onCleared]);

  return <div ref={containerRef} />;
};

export default EraserCanvas;
