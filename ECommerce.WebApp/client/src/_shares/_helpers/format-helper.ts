export class FormatHelper {
  public static getNumber(inputString?: string) {
    if (inputString) {
      // Remove non-numeric characters (everything except digits and a decimal point)
      const cleanedString = inputString.replace(/[^0-9.]/g, "");
      // Convert the cleaned string to a number
      const number = parseFloat(cleanedString);
      return number;
    }
    return null;
  }
}
