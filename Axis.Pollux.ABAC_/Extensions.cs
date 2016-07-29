using Axis.Luna.Extensions;

using System;
using sigma = Axis.Sigma.Core;
using pollux = Axis.Pollux.ABAC.Auth;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;

namespace Axis.Pollux.ABAC
{
    public static class Extensions
    {
        #region Sigma Casts
        public static sigma.Attribute ToSigma(this pollux.AuthorizationAttribute attribute)
        {
            sigma.Attribute sigatt = null;
            if (attribute is pollux.IntegralAttribute)
                sigatt = new sigma.Attribute<long>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.IntegralAttribute>().Value };

            else if (attribute is pollux.BinaryAttribute)
                sigatt = new sigma.Attribute<byte[]>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.BinaryAttribute>().Value };

            else if (attribute is pollux.BooleanAttribute)
                sigatt = new sigma.Attribute<bool>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.BooleanAttribute>().Value };

            else if (attribute is pollux.DateTimeAttribute)
                sigatt = new sigma.Attribute<DateTime?>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.DateTimeAttribute>().Value };

            else if (attribute is pollux.DecimalAttribute)
                sigatt = new sigma.Attribute<decimal>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.DecimalAttribute>().Value };

            else if (attribute is pollux.GuidAttribute)
                sigatt = new sigma.Attribute<Guid>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.GuidAttribute>().Value };

            else if (attribute is pollux.RealAttribute)
                sigatt = new sigma.Attribute<double>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.RealAttribute>().Value };

            else if (attribute is pollux.StringAttribute)
                sigatt = new sigma.Attribute<string>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.StringAttribute>().Value };

            else if (attribute is pollux.TimeSpanAttribute)
                sigatt = new sigma.Attribute<TimeSpan?>(attribute.Category.ToSigma()) { Value = attribute.As<pollux.TimeSpanAttribute>().Value };


            sigatt.Name = attribute.Name;
            return sigatt;
        }
        public static sigma.Policy.PolicySet ToSigma(this pollux.PolicySet policySet)
        {
            if (policySet == null) return null;
            else return new sigma.Policy.PolicySet
            {
                Id = policySet.Name,
                Title = policySet.Title,
                CombinationClause = policySet.CombinationClause.AsCombinationClause(),
                SubPolicies = policySet.Policies.Select(ps => ps.ToSigma()),
                SubSets = policySet.SubSets.Select(ps => ps.ToSigma()),
                Target = new sigma.Policy.PolicyTarget { Condition = policySet.TargetExpression.ToTargetExpression() }
            };
        }
        public static sigma.Policy.Policy ToSigma(this pollux.Policy policy)
        {
            if (policy == null) return null;
            else return new sigma.Policy.Policy
            {
                Id = policy.Name,
                Title = policy.Title,
                CombinationClause = policy.CombinationClause.AsCombinationClause(),
                Rules = policy.Rules.Select(r => r.ToSigma()),
                Target = new sigma.Policy.PolicyTarget { Condition = policy.TargetExpression.ToTargetExpression() }
            };
        }
        public static sigma.Policy.Rule ToSigma(this pollux.Rule rule)
        {
            if (rule == null) return null;
            else return new sigma.Policy.Rule
            {
                Id = rule.Name,
                ActionCondition = new sigma.ActionExpression { Expression = rule.ActionCondition.ToAttributeExpression<sigma.Action>() },
                EnvironmentCondition = new sigma.EnvironmentExpression { Expression = rule.ActionCondition.ToAttributeExpression<sigma.Environment>() },
                ResourceCondition = new sigma.ResourceExpression { Expression = rule.ActionCondition.ToAttributeExpression<sigma.Resource>() },
                SubjectCondition = new sigma.SubjectExpression { Expression = rule.ActionCondition.ToAttributeExpression<sigma.Subject>() }
            };
        }
        public static sigma.AttributeCategory ToSigma(this pollux.Category category)
        {
            if (pollux.Category.Action.Equals(category)) return sigma.AttributeCategory.Action;
            else if (pollux.Category.Environment.Equals(category)) return sigma.AttributeCategory.Environment;
            else if (pollux.Category.Resource.Equals(category)) return sigma.AttributeCategory.Resource;
            else if (pollux.Category.Environment.Equals(category)) return sigma.AttributeCategory.Subject;
            else throw new Exception("invalid category");
        }
        #endregion

        #region Pollux Cast
        public static pollux.AuthorizationAttribute ToPollux(this sigma.Attribute attribute)
        {
            throw new Exception();
        }
        public static pollux.PolicySet ToPollux(this sigma.Policy.PolicySet policySet)
        {

        }
        public static pollux.Policy ToPollux(this sigma.Policy.Policy policySet)
        {

        }
        public static pollux.Rule ToPollux(this sigma.Policy.Rule policySet)
        {

        }
        public static pollux.Category ToPollux(this sigma.AttributeCategory category)
        {
            switch(category)
            {
                case sigma.AttributeCategory.Action: return pollux.Category.Action;
                case sigma.AttributeCategory.Environment: return pollux.Category.Environment;
                case sigma.AttributeCategory.Resource: return pollux.Category.Resource;
                case sigma.AttributeCategory.Subject:
                default: return pollux.Category.Subject;
            }
        }
        #endregion

        public static string AsString(this sigma.Policy.ICombinationClause clause)
            => JsonConvert.SerializeObject(new JsonContainer { Type = clause.GetType().MinimalAQName(), Json = JsonConvert.SerializeObject(clause) });

        public static sigma.Policy.ICombinationClause AsCombinationClause(this string value)
        {
            var jc = JsonConvert.DeserializeObject<JsonContainer>(value);
            return JsonConvert.DeserializeObject(jc.Json, jc.ClrType()).As<sigma.Policy.ICombinationClause>();
        }

        public class JsonContainer
        {
            public Type ClrType() => System.Type.GetType(Type);
            public string Type { get; set; }
            public string Json { get; set; }
        }

        public static Expression<Func<sigma.Subject, sigma.Action, sigma.Resource, bool>> ToTargetExpression(this string expression)
        {

        }
        public static Expression<Func<V, bool>> ToAttributeExpression<V>(this string expression)
        {

        }
    }
}
