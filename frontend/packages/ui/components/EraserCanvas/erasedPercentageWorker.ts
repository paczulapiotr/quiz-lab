// erasedPercentageWorker.js

self.onmessage = function (e) {
  const { imageData, totalPixels, clearPercentage } = e.data;
  let erasedPixels = 0;
  for (let i = 0; i < imageData.data.length; i += 4) {
    if (imageData.data[i + 3] === 0) {
      erasedPixels++;
    }
  }
  const erasedPercentage = (erasedPixels / totalPixels) * 100;
  if (erasedPercentage >= clearPercentage) {
    self.postMessage(true);
  } else {
    self.postMessage(false);
  }
};
