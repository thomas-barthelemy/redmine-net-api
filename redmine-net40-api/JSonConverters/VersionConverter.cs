/*
   Copyright 2011 - 2015 Adrian Popescu

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Types;
using Version = Redmine.Net.Api.Types.Version;

namespace Redmine.Net.Api.JSonConverters
{
    internal class VersionConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        public override object Deserialize(
            IDictionary<string, object> dictionary,
            Type type,
            JavaScriptSerializer serializer)
        {
            if (dictionary == null) return null;
            var version = new Version
            {
                Id = dictionary.GetValue<int>("id"),
                Description = dictionary.GetValue<string>("description"),
                Name = dictionary.GetValue<string>("name"),
                CreatedOn = dictionary.GetValue<DateTime?>("created_on"),
                UpdatedOn = dictionary.GetValue<DateTime?>("updated_on"),
                DueDate = dictionary.GetValue<DateTime?>("due_date"),
                Project = dictionary.GetValueAsIdentifiableName("project"),
                Sharing = dictionary.GetValue<VersionSharing>("sharing"),
                Status = dictionary.GetValue<VersionStatus>("status"),
                CustomFields =
                    dictionary.GetValueAsCollection<IssueCustomField>("custom_fields")
            };
            return version;
        }

        public override IDictionary<string, object> Serialize(
            object obj,
            JavaScriptSerializer serializer)
        {
            var entity = obj as Version;
            var root = new Dictionary<string, object>();
            var result = new Dictionary<string, object>();
            if (entity != null)
            {
                result.Add("name", entity.Name);
                result.Add("status", entity.Status.ToString());
                result.Add("sharing", entity.Sharing.ToString());
                result.Add("description", entity.Description);
                result.WriteIfNotDefaultOrNull(entity.DueDate, "due_date");
                root["version"] = result;
                return root;
            }
            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>(new[] {typeof (Version)}); }
        }

        #endregion
    }
}