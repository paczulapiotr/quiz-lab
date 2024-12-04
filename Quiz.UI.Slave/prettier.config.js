import prettierConfig from "quiz-common-ui/dist/ide/prettier.config.js";

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
