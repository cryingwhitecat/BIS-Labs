import { env } from "process";
import { caesarShift, findKey, findKeyLetter } from "./analysis";
import { ShiftDirection } from "./enums/shift-direction.enum";
import { environment } from "./environment";
import { findKeyLength } from "./kasisky-exam";
import { textToBlocks, transformSplitted } from "./text-parsing";
import { readTextFromFile, writeToFile } from "./utils";

function decypher(text: string, key: string): string {
  let alphabet: string[] = Object.keys(environment.freqMap);
  let shifts: number[] = [...key].map((letter) => alphabet.indexOf(letter));
  let blocks: string[] = textToBlocks(text, key.length);
  let columns: string[] = transformSplitted(blocks);
  let decryptedBlocks: string[] = [];
  decryptedBlocks = columns.map((column, index) =>
    caesarShift(columns[index], shifts[index])
  );
  return transformSplitted(decryptedBlocks).join("").substring(0, text.length);
}

function cypherToFile(inputPath: string, outputPath: string, key: string): void {
  let cyphertext: string = "";
  let alphabet = Object.keys(environment.freqMap);
  const text = [...readTextFromFile(inputPath).toUpperCase()].filter(x => alphabet.includes(x));
  let keyLength = key.length;
  [...text].forEach((letter, index) => {
    cyphertext += caesarShift(
      letter,
      alphabet.indexOf(key[index % keyLength]),
      ShiftDirection.Forward
    );
  });
  writeToFile(outputPath, cyphertext);
}

function hack(inputPath: string, outputPath: string): void {
  const text = readTextFromFile(inputPath);
  const keyLength = findKeyLength(
    text,
    environment.fragmentLen,
    environment.maxKeyLength,
    environment.minKeyLength
  );
  const key = findKey(text, keyLength);
  const decyphered = decypher(text, key);
  console.log(`Found key: ${key}`);
  writeToFile(outputPath, decyphered);
}

cypherToFile("text/cleartext.txt", "text/encoded.txt", "RUDENKO");
hack("text/encoded.txt", "text/decoded.txt");