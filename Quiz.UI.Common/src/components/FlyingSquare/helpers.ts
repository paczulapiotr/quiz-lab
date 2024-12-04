const getRandomBorderPosition = (squareSize: number) => {
  const position: { top?: string; left?: string } = {};
  const randomValue = Math.random();

  if (randomValue < 0.25) {
    // Top border
    position.top = `-${squareSize}px`;
    position.left = Math.random() * window.innerWidth + "px";
  } else if (randomValue < 0.5) {
    // Right border
    position.top = Math.random() * window.innerHeight + "px";
    position.left = window.innerWidth + "px";
  } else if (randomValue < 0.75) {
    // Bottom border
    position.top = window.innerHeight + "px";
    position.left = Math.random() * window.innerWidth + "px";
  } else {
    // Left border
    position.top = Math.random() * window.innerHeight + "px";
    position.left = `-${squareSize}px`;
  }

  return position;
};

export const flySquare = (square: HTMLDivElement) => {
  const squareSize = square.offsetWidth;
  const startPosition = getRandomBorderPosition(squareSize);
  const endPosition = getRandomBorderPosition(squareSize);

  square.style.top = startPosition.top!;
  square.style.left = startPosition.left!;

  const duration = 30_000; // 5 seconds
  const startTime = performance.now();

  const animate = (time: number) => {
    const elapsed = time - startTime;
    const progress = Math.min(elapsed / duration, 1);

    square.style.top = `calc(${startPosition.top} + (${parseFloat(endPosition.top!) - parseFloat(startPosition.top!)}px) * ${progress})`;
    square.style.left = `calc(${startPosition.left} + (${parseFloat(endPosition.left!) - parseFloat(startPosition.left!)}px) * ${progress})`;

    if (progress < 1) {
      requestAnimationFrame(animate);
    } else {
      flySquare(square);
    }
  };

  requestAnimationFrame(animate);
};
