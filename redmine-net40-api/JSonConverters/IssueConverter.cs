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
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api.JSonConverters
{
    internal class IssueConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        public override object Deserialize(
            IDictionary<string, object> dictionary,
            Type type,
            JavaScriptSerializer serializer)
        {
            if (dictionary == null) return null;
            var issue = new Issue
            {
                Id = dictionary.GetValue<int>("id"),
                Description = dictionary.GetValue<string>("description"),
                Project = dictionary.GetValueAsIdentifiableName("project"),
                Tracker = dictionary.GetValueAsIdentifiableName("tracker"),
                Status = dictionary.GetValueAsIdentifiableName("status"),
                CreatedOn = dictionary.GetValue<DateTime?>("created_on"),
                UpdatedOn = dictionary.GetValue<DateTime?>("updated_on"),
                ClosedOn = dictionary.GetValue<DateTime?>("closed_on"),
                Priority = dictionary.GetValueAsIdentifiableName("priority"),
                Author = dictionary.GetValueAsIdentifiableName("author"),
                AssignedTo = dictionary.GetValueAsIdentifiableName("assigned_to"),
                Category = dictionary.GetValueAsIdentifiableName("category"),
                FixedVersion = dictionary.GetValueAsIdentifiableName(
                    "fixed_version"),
                Subject = dictionary.GetValue<string>("subject"),
                Notes = dictionary.GetValue<string>("notes"),
                StartDate = dictionary.GetValue<DateTime?>("start_date"),
                DueDate = dictionary.GetValue<DateTime?>("due_date"),
                DoneRatio = dictionary.GetValue<float>("done_ratio"),
                EstimatedHours = dictionary.GetValue<float>("estimated_hours"),
                ParentIssue = dictionary.GetValueAsIdentifiableName("parent"),
                CustomFields =
                    dictionary.GetValueAsCollection<IssueCustomField>("custom_fields"),
                Attachments =
                    dictionary.GetValueAsCollection<Attachment>("attachments"),
                Relations =
                    dictionary.GetValueAsCollection<IssueRelation>("relations"),
                Journals = dictionary.GetValueAsCollection<Journal>("journals"),
                Changesets = dictionary.GetValueAsCollection<ChangeSet>(
                    "changesets"),
                Watchers = dictionary.GetValueAsCollection<Watcher>("watchers"),
                Children = dictionary.GetValueAsCollection<IssueChild>("children")
            };
            return issue;
        }

        public override IDictionary<string, object> Serialize(
            object obj,
            JavaScriptSerializer serializer)
        {
            var entity = obj as Issue;
            var root = new Dictionary<string, object>();
            var result = new Dictionary<string, object>();
            if (entity != null)
            {
                result.Add("subject", entity.Subject);
                result.Add("description", entity.Description);
                result.Add("notes", entity.Notes);
                result.WriteIdIfNotNull(entity.Project, "project_id");
                result.WriteIdIfNotNull(entity.Priority, "priority_id");
                result.WriteIdIfNotNull(entity.Status, "status_id");
                result.WriteIdIfNotNull(entity.Category, "category_id");
                result.WriteIdIfNotNull(entity.Tracker, "tracker_id");
                result.WriteIdIfNotNull(entity.AssignedTo, "assigned_to_id");
                result.WriteIdIfNotNull(entity.FixedVersion, "fixed_version_id");
                // result.WriteIdIfNotNull(entity.ParentIssue, "parent_issue_id");
                result.WriteIfNotDefaultOrNull(entity.EstimatedHours, "estimated_hours");
                if (entity.ParentIssue == null)
                    result.Add("parent_issue_id", null);
                else
                {
                    result.Add("parent_issue_id", entity.ParentIssue.Id);
                }
                if (entity.StartDate != null)
                    result.Add(
                        "start_date",
                        entity.StartDate.Value.ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture));
                if (entity.DueDate != null)
                    result.Add(
                        "due_date",
                        entity.DueDate.Value.ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture));
                result.WriteIfNotDefaultOrNull(entity.DoneRatio, "done_ratio");
                if (entity.Uploads != null)
                    result.Add("uploads", entity.Uploads.ToArray());
                if (entity.CustomFields != null)
                {
                    serializer.RegisterConverters(new[] {new IssueCustomFieldConverter()});
                    result.Add("custom_fields", entity.CustomFields.ToArray());
                }
                if (entity.Watchers != null)
                {
                    serializer.RegisterConverters(new[] {new WatcherConverter()});
                    result.Add("watcher_user_ids", entity.Watchers.ToArray());
                }
                root["issue"] = result;
                return root;
            }
            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>(new[] {typeof (Issue)}); }
        }

        #endregion
    }
}