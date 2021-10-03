import * as fs from "fs";
import * as path from "path";

export function countIn<T>(array: Array<T>, element: T): number {
  return array.reduce((prev, curr) => (curr === element ? prev + 1 : prev), 0);
}

export function readTextFromFile(filePath: string): string {
  return fs.readFileSync(path.join(__dirname, filePath), {
    encoding: "utf-8",
  });
}

export function writeToFile(filePath: string, text: string): void {
  fs.writeFileSync(path.join(__dirname, filePath), text);
}
