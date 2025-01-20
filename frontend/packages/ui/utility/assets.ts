export const prefetchResource = (url:string) => {
  return new Promise((resolve, reject) => {
    const link = document.createElement('link');
    link.rel = 'prefetch';
    link.href = url;

    link.onload = () => resolve(null);

    link.onerror = () => reject(new Error(`Failed to prefetch resource: ${url}`));

    document.head.appendChild(link);
  });
};