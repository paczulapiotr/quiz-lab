export type KeyboardOptions = {
  disableShift?: boolean;
  disableSpaceBar?: boolean;
  disableBackspace?: boolean;
};

export const mapLayout = ({
  disableSpaceBar,
  disableBackspace,
  disableShift,
}: KeyboardOptions) => {
  const baseDefault = [
    "ą ć ę ł ń ó ś ź ż",
    "q w e r t y u i o p",
    "a s d f g h j k l",
    "z x c v b n m",
  ];

  const def = baseDefault.map((row) => row);
  const shift = baseDefault.map((row) => row.toUpperCase());

  if (!disableSpaceBar) {
    def.push("{space}");
    shift.push("{space}");
  }

  if (!disableBackspace) {
    def[0] = `${def[0]} {bksp}`;
    shift[0] = `${shift[0]} {bksp}`;
  }

  if (!disableShift) {
    def[3] = `{shift} ${def[3]}`;
    shift[3] = `{shift} ${shift[3]}`;
  }

  return {
    default: def,
    shift,
  };
};

export const SpecialButtonKeys = ["{bksp}", "{shift}", "{space}"];
