import { countIn } from "./utils";

export function findRepeatingStrings(
  text: string,
  substringLength: number
): { [key: string]: number[] } {
  let stringsPos: { [key: string]: number[] } = {};
  let repeatingStringsPos: { [key: string]: number[] } = {};
  [...text].forEach((_, index) => {
    const substring = text.substring(index, index + substringLength);
    if (!!stringsPos[substring]) {
      stringsPos[substring].push(index);
    } else {
      stringsPos[substring] = [index];
    }
  });
  Object.keys(stringsPos)
    .filter((key) => stringsPos[key].length >= 2)
    .forEach((letter) => (repeatingStringsPos[letter] = stringsPos[letter]));
  return repeatingStringsPos;
}

export function findDistanceBetweenRepeating(
  repeatingStringPositions: number[]
): number[] {
  return repeatingStringPositions
    .slice(0, repeatingStringPositions.length - 1)
    .map((_, i, arr) => {
      return repeatingStringPositions[i + 1] - repeatingStringPositions[i];
    });
}

export function factorize(number: number): number[] {
  let factors = new Set<number>();
  for (let i = 1; i < Math.round(Math.sqrt(number)) + 1; i++) {
    if (number % i === 0) {
      factors.add(i);
      factors.add(Math.round(number / i));
    }
  }
  return [...factors].sort();
}

export function findCandidateKeyLengths(
  factors: Array<Array<number>>,
  maxKeyLength: number,
  minKeyLength: number
) {
  let flatFactors = factors
    .reduce((prev, current) => prev.concat(current))
    .filter((x) => x >= minKeyLength && x <= maxKeyLength);
  return flatFactors.sort(
    (a, b) => -1 * (countIn(flatFactors, a) - countIn(flatFactors, b))
  );
}

export function findKeyLength(
  text: string,
  stringLength,
  maxKeyLength,
  minKeyLength
): number {
  let repeatingStrings = findRepeatingStrings(text, stringLength);
  let repeatingDistances: { [key: string]: number[] } = {};
  Object.keys(repeatingStrings).forEach(
    (key) =>
      (repeatingDistances[key] = findDistanceBetweenRepeating(
        repeatingStrings[key]
      ))
  );
  let factorLists = new Array<Array<number>>();
  Object.keys(repeatingDistances).forEach((key) => {
    repeatingDistances[key].forEach((distance) => {
      factorLists.push(factorize(distance));
    });
  });
  let candidateLengths: number[] = findCandidateKeyLengths(
    factorLists,
    maxKeyLength,
    minKeyLength
  );
  return candidateLengths[0];
}
