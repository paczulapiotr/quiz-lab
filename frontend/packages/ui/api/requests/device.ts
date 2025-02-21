import { AxiosResponse } from "axios";
import instance from "../instance";

export type GetDeviceResponse = {
  deviceId?: string;
  hostId?: string;
  roomCode?: string;
};

export const getDevice = async () =>
  (await instance.get<never, AxiosResponse<GetDeviceResponse>>(`/device`)).data;

export type RegisterDeviceRequest = {
  roomCode: string;
  isHost: boolean;
};
export type RegisterDeviceResponse = {
  roomCode?: string;
  uniqueId?: string;
  ok: boolean;
};

export const registerDevice = async (
  roomCode: string,
  isHost: boolean = false
) =>
  (
    await instance.post<
      never,
      AxiosResponse<RegisterDeviceResponse>,
      RegisterDeviceRequest
    >("/device/register", {
      roomCode,
      isHost,
    })
  ).data;
