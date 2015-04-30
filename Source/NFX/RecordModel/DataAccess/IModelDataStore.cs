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
 * Originated: 2008.03
 * Revision: NFX 1.0  2011.02.06
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX.Environment;
using NFX.DataAccess;


namespace NFX.RecordModel.DataAccess
{
  /// <summary>
  /// Represents a store that can save and retrieve data for controllers
  /// </summary>
  public interface IModelDataStore : IDataStore
  {
     
     /// <summary>
     /// Loads instance data from store
     /// </summary>
     /// <param name="instance">Model instance to fill with data</param>
     /// <param name="key">A key object that uniquely identifies what data to load (such as COUNTER)</param>
     /// <param name="extra">Extra parameters passed to a store</param>
     void Load(ModelBase instance, IDataStoreKey key, params object[] extra);
     
     /// <summary>
     /// Saves data from model instance into a store
     /// </summary>
     /// <param name="instance">Model instance to be saved</param>
     /// <param name="key">Optional key to store data under (may be null i.e. for identity columns or if data inlined in model itself)</param>
     /// <param name="extra">Extra params passed to a store</param>
     void Save(ModelBase instance, IDataStoreKey key, params object[] extra);
     
     
     /// <summary>
     /// Deletes data for model of specified type (passed as instance) with specified key
     /// </summary>
     void Delete(ModelBase instance, IDataStoreKey key, params object[] extra);
     
  }
  

   public interface IModelDataStoreImplementation : IModelDataStore, IDataStoreImplementation
   {

   }
  
}
