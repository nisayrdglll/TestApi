// using System;
// using System.Collections.Generic;
// using DataAccess.Models;
// using DataAccess.Schemas;
// using DataAccess.Context;

// namespace Helpers.Helpers
// {
//     public class Helper
//     {
// 		private readonly DbContext _context;
// 		public Helper(DbContext context)
//         {
//             _context = context;
// 		}
//         public static string DetermineFieldNameFromModel(string errorMessage, object model)
//         {
//             var properties = model.GetType().GetProperties();

//             foreach (var property in properties)
//             {
//                 if (errorMessage.Contains(property.Name))
//                 {
//                     return property.Name;
//                 }
//             }
//             return errorMessage;
//         }

//         public async Task<List<Tanimlama>> GetTanimlamaListByGrupAdi(
//             bool addDeletedFilter,
//             bool addEnabledFilter,
//             string GrupAdi)
//         {
//             var filterList = new List<FilterSchema>();
//             var filter = new FilterSchema();

//             if (addDeletedFilter)
//             {
//                 filter.Field = "Deleted";
//                 filter.Operator = "==";
//                 filter.Value = "false";

//                 filterList.Add(filter);
//             }

//             if (addEnabledFilter)
//             {
//                 filter.Field = "Aktif";
//                 filter.Operator = "==";
//                 filter.Value = "true";

//                 filterList.Add(filter);
//             }

// 			filter.Field = "GrupAdi";
// 			filter.Operator = "==";
// 			filter.Value = GrupAdi;

// 			filterList.Add(filter);

// 			var tanimlamaObjects = await _unitOfWork.TanimlamaRepository.GetAsync(
//                     "",
//                     "",
//                     filterList,
//                     "Kod",
//                     0,
// 					0);

// 			return (List<Tanimlama>)tanimlamaObjects;
//         }
//     }
// }