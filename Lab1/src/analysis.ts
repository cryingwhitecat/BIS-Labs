import { LetterStatType } from "./enums/letter-stat-type.enum";
import { ShiftDirection } from "./enums/shift-direction.enum";
import { environment } from "./environment";
import { textToBlocks, transformSplitted } from "./text-parsing";
import { countIn } from "./utils";

export function getLetterStats(
  text: string,
  statType: LetterStatType = LetterStatType.Frequency
): { [key: string]: number } {
  const toUpper = [...text.toUpperCase()];
  const textLength = text.length;

  let letterCountsMap: { [key: string]: number } = {};
  Object.keys(environment.freqMap).forEach((x) => {
    letterCountsMap[x] = countIn(toUpper, x);
  });

  if (statType === LetterStatType.Count) {
    return letterCountsMap;
  } else {
    let letterFreqMap: { [key: string]: number } = {};
    Object.keys(letterCountsMap).forEach((letter) => {
      letterFreqMap[letter] = letterCountsMap[letter] / textLength;
    });
    return letterFreqMap;
  }
}

export function caesarShift(text: string, shift: number, shiftDirection: ShiftDirection = ShiftDirection.Backward): string {
  let shifted = "";
  let alphabet = Object.keys(environment.freqMap);
  for (let letter of text) {
      if(shiftDirection == ShiftDirection.Backward){
        let shiftIndex = (alphabet.indexOf(letter) - shift) % alphabet.length;
        shiftIndex = shiftIndex < 0 ? alphabet.length + shiftIndex : shiftIndex;
        let temp = alphabet[shiftIndex];
        shifted += temp;
      }else{
        let shiftIndex = (alphabet.indexOf(letter) + shift) % alphabet.length;
        let temp = alphabet[shiftIndex];
        shifted += temp;
      }

  }
  return shifted;
}

export function corellateWithStats(
  text: string,
  letterFreqMap: { [key: string]: number }
): number {
  let sum: number = 0;
  for (let letter of text) {
    let temp = letterFreqMap[letter] * environment.freqMap[letter];
    sum += temp;
  }
  return sum;
}

export function findKeyLetter(text: string, letterFreqMap): string {
  let key: string = "";
  let maxCorrelation = 0;
  Object.keys(environment.freqMap).forEach((letter, index) => {
    let temp = caesarShift(text, index);
    let corellation = corellateWithStats(temp, letterFreqMap);
    if (corellation > maxCorrelation) {
      maxCorrelation = corellation;
      key = letter;
    }
  });
  return key;
}

export function findKey(text: string, keyLength: number): string {
  let key: string = "";
  let blocks: string[] = textToBlocks(text, keyLength);
  let columns: string[] = transformSplitted(blocks);
  let frequencyMap = getLetterStats(text, LetterStatType.Frequency);
  columns.forEach((column) => (key += findKeyLetter(column, frequencyMap)));
  return key;
}
