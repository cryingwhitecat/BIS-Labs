export function textToBlocks(text: string, blockLength: number): string[] {
  let blocks = new Array<string>();
  let i = 0;
  for (; i < text.length - blockLength; i += blockLength) {
    blocks.push(text.substring(i, i + blockLength));
  }
  if(i != text.length){
      blocks.push(text.substring(i));
  }
  return blocks;
}

export function transformSplitted(splittedText: string[]): string[] {
  let size = splittedText[0].length;
  let result = new Array<string>();
  for (let i = 0; i < size; i++) {
    let temp = "";
    for (let j = 0; j < splittedText.length; j++) {
      temp += splittedText[j][i];
    }
    result.push(temp);
  }
  return result;
}
