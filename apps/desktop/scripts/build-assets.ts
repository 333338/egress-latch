import { copyFileSync, mkdirSync } from "node:fs";
import path from "node:path";

const dir = path.resolve(import.meta.dirname, "../assets");
mkdirSync(dir, { recursive: true });
copyFileSync(
  path.resolve(
    import.meta.dirname,
    "../../web/node_modules/@fontsource-variable/noto-sans-sc/LICENSE",
  ),
  path.join(dir, "Noto-Sans-SC-OFL.txt"),
);
