using System.Text;
using UnityEngine;
public static class CommonUtilities
{
    public static string FormatString(string originalString, int maxLength)
    {
		string newString = string.Empty;

		// Check if the string is short to begin with.
		if (originalString.Length < maxLength)
		{
			newString = originalString;
			return newString;
		}

		if (originalString.Length > maxLength)
        {
			Debug.LogWarning($"[FormatString] was given a string longer than max of {maxLength} characters.");
        }

		// Otherwise, proceed to format
		int lastWhiteSpaceIndex = 0;
		int startIndex = 0;
		StringBuilder sb = new StringBuilder();

		for (int i = 0, j = 0; i < originalString.Length; i++, j++)
		{
			char c = originalString[i];
			bool isWhiteSpace = char.IsWhiteSpace(c);

			// Check if we're close to end. Append remainder if close.
			if (i + 1 >= originalString.Length)
			{
				sb.Append(originalString.Substring(startIndex).Trim());
				break;
			}

			// Cache this index if a whitespace.
			if (isWhiteSpace)
			{
				lastWhiteSpaceIndex = i;
			}

			// Check if new line is ready. Continue loop if not.
			if (lastWhiteSpaceIndex == -1 && j < maxLength)
			{
				continue;
			}

			// When j reaches the maximum length, check if whitespace. Append.
			if (j == maxLength)
			{
				// If White space, or no whitespace, append. But trim.
				if (isWhiteSpace || lastWhiteSpaceIndex == startIndex)
				{
					sb.AppendLine(originalString.Substring(startIndex, maxLength).Trim());
					j = 0;
					startIndex = i;
					lastWhiteSpaceIndex = startIndex;
					continue;
				}

				j = -1;
				int size = lastWhiteSpaceIndex - startIndex;
				sb.AppendLine(originalString.Substring(startIndex, size + 1).Trim());

				// Next index
				startIndex += size;
				i = startIndex;
				lastWhiteSpaceIndex = i;
			}
			newString = sb.ToString();
		}

		return newString;
	}
}
