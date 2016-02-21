/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2014 IT Adapter Inc / 2015 Aum Code LLC
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/


/* NFX by ITAdapter
 * Originated: 2006.01
 * Revision: NFX 1.0  2011.02.03
 */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NFX.Parsing
{
  /// <summary>
  /// Provides misc data-entry parsing routines
  /// </summary>
  public static class DataEntryUtils
  {


    /// <summary>
    /// Returns true if the value is a valid non-null/empty email address
    /// </summary>
    public static bool CheckEMail(string email)
    {
      if (email.IsNullOrWhiteSpace()) return false;

      var lastPos = email.Length - 1;

      var atPos = email.IndexOf('@');
      if (atPos <= 0 || atPos == lastPos)
        return false;

      char c;

      // local part
      var prevIsDot = false;
      var end = atPos - 1;
      for (int i = 0; i <= end; i++)
      {
        c = email[i];
        if (i == 0 || i == end)
        {
          if (!IsValidEMailLocalPartChar(c)) return false;
          continue;
        }
        if (c == '.')
        {
          if (prevIsDot) return false;
          prevIsDot = true;
        }
        else 
        {
          if (!IsValidEMailLocalPartChar(c)) return false;
          prevIsDot = false;
        }
      }

      // domain part
      var prevIsHyphen = false;
      prevIsDot = false;
      var start = atPos + 1;
      end = lastPos;
      for (int i = start; i <= end; i++)
      {
        c = email[i];
        if (i == start || i == end)
        {
          if (!Char.IsLetterOrDigit(c)) return false;
          continue;
        }
        if (c == '.')
        {
          if (prevIsDot || prevIsHyphen) return false;
          prevIsDot = true;
          continue;
        }
        if (c == '-')
        {
          if (prevIsDot) return false;
          prevIsHyphen = true;
          continue;
        }
        if (!Char.IsLetterOrDigit(c)) return false;
        prevIsDot = false;
        prevIsHyphen = false;
      }
      return true;
    }

    /// <summary>
    /// Returns true if the value is a valid non-null/empty telephone
    /// </summary>
    public static bool CheckTelephone(string phone)
    {
      if (phone.IsNullOrWhiteSpace())
        return false;
      
      phone = phone.Trim();
      if (phone.Length == 0) return false;

      char c;
      var meetFirstParenthesis = false;
      var meetSecondParenthesis = false;
      for (int i = 0; i < phone.Length; i++)
      {
        c = phone[i];
        if (i == 0 && c == '+') continue;
        if (c == '(' && !meetFirstParenthesis && !meetSecondParenthesis)
        {
          meetFirstParenthesis = true;
          continue;
        }
      }
       // the phone may start with + "international", it may only have 1 plus at the very beginning
       // the "(" must be matched with ")" , may not start from ")"
       // may contain 0..9 A-Z and '-' or '.'
       // cant have more than one consecuitive '-' or '.' also cant have -. or .-
       // 818.234.2314x518
       // (818) 234-2314 ext 518
       // (818) 234-2314x518
       // 818-234-2314x518
       return true;
    }

    
    /// <summary>
    /// Returns true if the value starts from primary language char and contains only those chars separated by one of ['.','-','_'].
    /// Subsequent separators not to occur more than once and can not be at the very end. This function supports Latin/Cyrrilic char sets
    /// </summary>
    public static bool CheckScreenName(string name)
    {
      if (name.IsNullOrWhiteSpace()) return false;
      name = name.Trim();
      if (name.Length==0) return false;
      var wasSeparator = false;
      for(var i=0; i<name.Length; i++)
      {
        var c = name[i];
        if (i==0)
        {
          if (!IsValidScreenNameLetter(c)) return false;
          continue;
        }
       
        if (IsValidScreenNameSeparator(c))
        {
          if (wasSeparator) return false;
          wasSeparator = true;
          continue;
        }
        if (!IsValidScreenNameLetterOrDigit(c)) return false;
        wasSeparator = false;
      }
      return !wasSeparator;
    }

    public static bool IsValidScreenNameLetter(char c)
    {
      const string extra = "ёЁÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥ";
      return (c>='A' && c<='Z') ||
             (c>='a' && c<='z') ||
             (c>='А' && c<='Я') ||
             (c>='а' && c<='я') ||
             (extra.IndexOf(c)>=0);
    }

    public static bool IsValidScreenNameLetterOrDigit(char c)
    {
      return IsValidScreenNameLetter(c) || (c>='0' && c<='9');
    }

    public static bool IsValidScreenNameSeparator(char c)
    {
      return (c=='.' || c=='-' || c=='_');
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidEMailLocalPartChar(char c)
    {
      const string validLocalPartChars = "!#$%&'*+-/=?^_`{|}~";
      return (Char.IsLetterOrDigit(c) || validLocalPartChars.IndexOf(c) >= 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLatinLetterOrDigit(char c)
    {
      return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
    }
        
    /// <summary>
    /// Normalizes US phone string so it looks like (999) 999-9999x9999.
    /// </summary>
    public static string NormalizeUSPhone(string value)
    {
      if (value == null) return null;

      value = value.Trim();

      if (value.Length == 0) return value;


      bool isArea = false;
      bool isExt = false;

      string area = string.Empty;
      string number = string.Empty;
      string ext = string.Empty;


      if (value.StartsWith("+")) //international phone, just return as-is
        return value;

      Char chr;
      for (int i = 0; i < value.Length; i++)
      {
        chr = value[i];

        if (!isArea && chr == '(' && area.Length == 0)
        {
          isArea = true;
          continue;
        }

        if (isArea && chr == ')')
        {
          isArea = false;
          continue;
        }

        if (isArea && area.Length == 3)
          isArea = false;


        if (number.Length > 0 && !isExt)
        {  //check extention
          if (chr == 'x' || chr == 'X' || (chr == '.' && number.Length>6))
          {
            isExt = true;
            continue;
          }
          string trailer = value.Substring(i).ToUpperInvariant();

          if (trailer.StartsWith("EXT") && number.Length >= 7)
          {
            isExt = true;
            i += 2;
            continue;
          }
          if (trailer.StartsWith("EXT.") && number.Length >= 7)
          {
            isExt = true;
            i += 3;
            continue;
          }

        }

        if (!Char.IsLetterOrDigit(chr)) continue;


        if (isArea)
          area += chr;
        else
          if (isExt)
            ext += chr;
          else
            number += chr;
      }



      while (number.Length < 7)
        number += '?';


      if (area.Length == 0)
      {
        if (number.Length >= 10)
        {
          area = number.Substring(0, 3);
          number = number.Substring(3);
        }
        else
          area = "???";
      }

      if (number.Length > 7 && ext.Length == 0)
      {
        ext = number.Substring(7);
        number = number.Substring(0, 7);
      }

      number = number.Substring(0, 3) + "-" + number.Substring(3);

      if (ext.Length > 0) ext = "x" + ext;

      return string.Format("({0}) {1}{2}", area, number, ext);
    }

  }
}
