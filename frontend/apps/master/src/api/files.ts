import path from 'path';

export const fileUrl = (filePath: string) => {
    return path.join(import.meta.env.VITE_LOCAL_FILES_URL, filePath);
};