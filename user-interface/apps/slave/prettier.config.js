import prettierConfig from "@repo/prettier-config";

/**
 * @see https://prettier.io/docs/en/configuration.html
 * @type {import("prettier").Config}
 */
const localConfig = {};

// Merge configurations
const mergedConfig = {
  ...prettierConfig,
  ...localConfig,
};
export default mergedConfig;
