import { FC, useEffect, useRef } from "react";
import {
  Application,
  Graphics,
  BLEND_MODES,
  LINE_CAP,
  LINE_JOIN,
} from "pixi.js";

const EraserCanvas: FC = () => {
  const canvasRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    // 1. Create the Pixi Application
    const app = new Application({
      width: 600,
      height: 400,
      backgroundAlpha: 0, // Transparent background behind
      antialias: true,
    });

    if (!canvasRef.current) return;
    canvasRef.current.appendChild(app.view as HTMLCanvasElement);

    // 2. Draw a colored background rectangle
    const background = new Graphics();
    background.beginFill(0xff0000); // Red fill
    background.drawRect(0, 0, app.screen.width, app.screen.height);
    background.endFill();
    app.stage.addChild(background);

    // 3. Interactivity setup
    app.stage.interactive = true;
    app.stage.hitArea = background.getLocalBounds();

    let isErasing = false;
    let lastPos: { x: number; y: number } | null = null;

    // 4. Start erasing on pointerdown
    app.stage.on("pointerdown", (event) => {
      isErasing = true;
      // Save the initial pointer position
      const { x, y } = event.data.global;
      lastPos = { x, y };
    });

    // 5. Stop erasing on pointerup
    app.stage.on("pointerup", () => {
      isErasing = false;
      lastPos = null;
    });

    // 6. On pointer move, draw a thick line with round caps between the last position and current position
    app.stage.on("pointermove", (event) => {
      if (!isErasing || !lastPos) return;

      // Current pointer position
      const { x, y } = event.data.global;

      // Create a line to erase between the last position and current position
      const eraserLine = new Graphics();
      // 7. Set line style (30px thick, round caps, round joins)
      eraserLine.lineStyle({
        width: 30,
        color: 0xffffff,
        alpha: 1,
        cap: LINE_CAP.ROUND,
        join: LINE_JOIN.ROUND,
      });
      eraserLine.blendMode = BLEND_MODES.ERASE;

      // Move from last position to current pointer position
      eraserLine.moveTo(lastPos.x, lastPos.y);
      eraserLine.lineTo(x, y);

      // Add it on top of the background so it can erase
      background.addChild(eraserLine);

      // Update last position
      lastPos = { x, y };
    });

    // 8. Cleanup on component unmount
    return () => {
      app.destroy(true, true);
    };
  }, []);

  return <div ref={canvasRef} />;
};

export default EraserCanvas;
