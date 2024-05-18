//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Core.Scripts.Localizations.Editor
{
    public static class LocalizationXlsxWriter
    {
        private static readonly string FilePath = Path.Combine(Application.dataPath, "localization.xlsx");
        private const int WidthColumn = 64 * 256;
        
        [MenuItem("Localization/Export to XLSX")]
        private static void WriteLocalizationInFile()
        {
            LocalizationEditor.Init();
            var languages = new List<Language>(LocalizationController.Languages);
            
            IWorkbook workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet("Sheet1");
            sheet.SetColumnWidth(0, WidthColumn);
            
            var row = sheet.CreateRow(0);

            var fontBold = workbook.CreateFont();
            fontBold.FontName = "Times New Roman";
            fontBold.IsBold = true;
            
            var cellStyleBold = workbook.CreateCellStyle();
            cellStyleBold.SetFont(fontBold);
            cellStyleBold.WrapText = true;

            var codeCell = row.CreateCell(0);
            codeCell.SetCellValue("Localization name");
            codeCell.CellStyle = cellStyleBold;
            
            for (var index = 0; index < languages.Count; index++)
            {
                sheet.SetColumnWidth(index + 1, WidthColumn);
                var cell = row.CreateCell(index + 1);
                cell.SetCellValue(languages[index].LanguageCode);
                cell.CellStyle = cellStyleBold;
                
                Debug.Log($"index - {index + 1}, language - {LocalizationController.Languages[index].LanguageName}");
            }

            var fontDefault = workbook.CreateFont();
            fontDefault.FontName = "Times New Roman";
            
            var cellStyleDefault = workbook.CreateCellStyle();
            cellStyleDefault.SetFont(fontDefault);
            cellStyleDefault.WrapText = true;
            
            for (var index = 0; index < LocalizationEditor.LocalizationProfile.LocalizationDates.Length; index++)
            {
                var rowLocalizations = sheet.CreateRow(index + 1);
                
                var localizationData = LocalizationEditor.LocalizationProfile.LocalizationDates[index];
                
                var code = rowLocalizations.CreateCell(0);
                
                code.SetCellValue(localizationData.LocalizationCode);
                code.CellStyle = cellStyleDefault;
                
                for (var index2 = 0; index2 < localizationData.Data.Count; index2++)
                {
                    var i = languages.FindIndex(language => language.LanguageCode == localizationData.Data[index2].Language.LanguageCode);

                    if (i == -1)
                    {
                        Debug.LogWarning($"index - {index + 1}, childIndex - {index2} - language incorrect");
                        continue;
                    }
                    
                    var cell = rowLocalizations.CreateCell(i + 1);
                    cell.SetCellValue(localizationData.Data[index2].Localization);
                    cell.CellStyle = cellStyleDefault;
                }
                
                Debug.Log($"index - {index + 1}, code - {localizationData.LocalizationCode}, localizations - {localizationData.Data.Count}");
            }

            using var fileStream = new FileStream(FilePath, FileMode.Create);
            
            workbook.Write(fileStream);
        }
    }
}
