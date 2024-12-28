import { FC, useEffect, useRef } from "react";
import { Application, Graphics, BLEND_MODES } from "pixi.js";

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
    canvasRef.current.appendChild(app.view as unknown as Node);

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

    // 4. Pointer events
    app.stage.on("pointerdown", () => {
      isErasing = true;
    });

    app.stage.on("pointerup", () => {
      isErasing = false;
    });

    app.stage.on("pointermove", (event) => {
      if (!isErasing) return;

      // Grab the pointer position
      const { x, y } = event.data.global;

      // 5. Create a square “eraser” graphic
      const eraser = new Graphics();
      eraser.beginFill(0xffffff);
      // Draw a 30x30 square centered at the pointer
      eraser.drawRect(x - 15, y - 15, 30, 30);
      eraser.endFill();

      // 6. Set blend mode to ERASE so it removes color
      eraser.blendMode = BLEND_MODES.ERASE;

      // 7. Add the eraser shape on top of the background
      //    Everything drawn with blendMode=ERASE will punch out transparent holes
      background.addChild(eraser);
    });

    // 8. Cleanup on component unmount
    return () => {
      app.destroy(true, true);
    };
  }, []);

  return <div ref={canvasRef} />;
};

export default EraserCanvas;
