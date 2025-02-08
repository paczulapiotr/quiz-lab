export const fileUrl = (filePath: string) => {
  return import.meta.env.VITE_LOCAL_FILES_URL + filePath;
};
