﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ancestor.Core;
using System.Reflection;

namespace Ancestor.DataAccess.DAO
{
    // 2015-10-22 修改 NOT IN 的方法, Create a private method for converting u to MethodCallExpression.
    public class LambdaExpressionHelper : ExpressionVisitor, IDisposable
    {
        public List<DBParameter> Parameters { get; set; }

        private StringBuilder sb;
        private string _orderBy = string.Empty;
        private int? _skip = null;
        private int? _take = null;
        private string _whereClause = string.Empty;

        private string _Symbolizer { get; set; }
        private string _Connector { get; set; }
        private string parameterString { get; set; }
        private string inParameterString { get; set; }
        private int parameterCount { get; set; }
        //public enum Symbolizer
        //{
        //    [Description("@")] SqlSymbolizer,
        //    [Description(":")] OracleSymbolizer
        //};

        //public enum Connector
        //{
        //    [Description("||")] OracleConnector,
        //    [Description("+")] SqlConnector
        //};

        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources.
            }
        }

        public int? Skip
        {
            get
            {
                return _skip;
            }
        }

        public int? Take
        {
            get
            {
                return _take;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderBy;
            }
        }

        public string WhereClause
        {
            get
            {
                return _whereClause;
            }
        }

        public LambdaExpressionHelper(string Symbolizer, string Connector)
        {
            _Symbolizer = Symbolizer;
            _Connector = Connector;
            Parameters = new List<DBParameter>();
            parameterCount = 0;
        }

        public string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            if (expression != null)
            {
                this.Visit(expression);
                // 2015-09-30 加入 WHERE 至回傳的回傳的字串中
                _whereClause = " WHERE " + this.sb.ToString();
            }
            return _whereClause;
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }


        protected void Write(object value)
        {
            this.sb.Append(value);
        }

        protected void AddParameter(DBParameter parameter)
        {
            Parameters.Add(parameter);
        }


        protected override Expression VisitMethodCall(MethodCallExpression m)
        {

            if (m.Method.DeclaringType == typeof(string))
            {
                switch (m.Method.Name)
                {
                    case "StartsWith":
                        this.Write("(");
                        this.Visit(m.Object);
                        this.Write(" LIKE " + parameterString);
                        this.Write(_Connector + "'%')");
                        SetParameter(m);
                        return m;
                    case "EndsWith":
                        this.Write("(");
                        this.Visit(m.Object);
                        this.Write(" LIKE '%'" + _Connector + parameterString);
                        this.Write(")");
                        SetParameter(m);
                        return m;
                    case "Contains":
                        this.Write("(");
                        this.Visit(m.Object);
                        this.Write(" LIKE '%'" + _Connector + parameterString);
                        this.Write(_Connector + "'%')");
                        SetParameter(m);
                        return m;
                    case "Concat":
                        IList<Expression> args = m.Arguments;
                        if (args.Count == 1 && args[0].NodeType == ExpressionType.NewArrayInit)
                        {
                            args = ((NewArrayExpression)args[0]).Expressions;
                        }
                        for (int i = 0, n = args.Count; i < n; i++)
                        {
                            if (i > 0) this.Write(_Connector);
                            this.Visit(args[i]);
                        }
                        return m;
                    case "IsNullOrEmpty":
                        this.Write("(");
                        this.Visit(m.Object);
                        this.Write(" IS NULL OR ");
                        this.Visit(m.Arguments[0]);
                        this.Write(" = '')");
                        //AddParameter(new DBParameter
                        //{
                        //    Name = ((MemberExpression)m.Object).Member.Name,
                        //    Type = m.Object.GetType().ToString(),
                        //    Size = 100,
                        //    ParameterDirection = ParameterDirection.Input,
                        //    Value = (m.Arguments[0] as ConstantExpression).Value
                        //});
                        SetParameter(m);
                        return m;
                    case "ToUpper":
                        this.Write("UPPER(");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "ToLower":
                        this.Write("LOWER(");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "Replace":
                        this.Write("REPLACE(");
                        this.Visit(m.Object);
                        this.Write(", ");
                        this.Visit(m.Arguments[0]);
                        this.Write(", ");
                        this.Visit(m.Arguments[1]);
                        this.Write(")");
                        return m;
                    case "Substring":
                        this.Write("SUBSTRING(");
                        this.Visit(m.Object);
                        this.Write(", ");
                        this.Visit(m.Arguments[0]);
                        this.Write(" + 1, ");
                        if (m.Arguments.Count == 2)
                        {
                            this.Visit(m.Arguments[1]);
                        }
                        else
                        {
                            this.Write("8000");
                        }
                        this.Write(")");
                        return m;
                    case "Remove":
                        this.Write("STUFF(");
                        this.Visit(m.Object);
                        this.Write(", ");
                        this.Visit(m.Arguments[0]);
                        this.Write(_Connector + "1, ");
                        if (m.Arguments.Count == 2)
                        {
                            this.Visit(m.Arguments[1]);
                        }
                        else
                        {
                            this.Write("8000");
                        }
                        this.Write(", '')");
                        return m;
                    case "IndexOf":
                        this.Write("(CHARINDEX(");
                        this.Visit(m.Arguments[0]);
                        this.Write(", ");
                        this.Visit(m.Object);
                        if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                        {
                            this.Write(", ");
                            this.Visit(m.Arguments[1]);
                            this.Write(_Connector + "1");
                        }
                        this.Write(") - 1)");
                        return m;
                    case "Trim":
                        this.Write("RTRIM(LTRIM(");
                        this.Visit(m.Object);
                        this.Write("))");
                        return m;
                }
            }
            else if (m.Method.DeclaringType.Name == "List`1")
            {
                switch (m.Method.Name)
                {
                    case "Contains":
                        this.Write("(");
                        var position = m.Arguments[0].ToString().IndexOf(".") + 1;
                        var length = m.Arguments[0].ToString().Length;
                        this.Write(m.Arguments[0].ToString().Substring(position, length - position));
                        this.Visit(m.Object);
                        this.Write(" in (" + inParameterString);
                        this.Write(") )");
                        inParameterString = string.Empty;
                        return m;
                }
            }
            else if (m.Method.DeclaringType == typeof(DateTime))
            {
                switch (m.Method.Name)
                {
                    case "op_Subtract":
                        if (m.Arguments[1].Type == typeof(DateTime))
                        {
                            this.Write("DATEDIFF(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", ");
                            this.Visit(m.Arguments[1]);
                            this.Write(")");
                            return m;
                        }
                        break;
                    case "AddYears":
                        this.Write("DATEADD(YYYY,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "AddMonths":
                        this.Write("DATEADD(MM,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "AddDays":
                        this.Write("DATEADD(DAY,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "AddHours":
                        this.Write("DATEADD(HH,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "AddMinutes":
                        this.Write("DATEADD(MI,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "AddSeconds":
                        this.Write("DATEADD(SS,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                    case "AddMilliseconds":
                        this.Write("DATEADD(MS,");
                        this.Visit(m.Arguments[0]);
                        this.Write(",");
                        this.Visit(m.Object);
                        this.Write(")");
                        return m;
                }
            }

            else if (m.Method.DeclaringType == typeof(Math))
            {
                switch (m.Method.Name)
                {
                    case "Abs":
                    case "Acos":
                    case "Asin":
                    case "Atan":
                    case "Cos":
                    case "Exp":
                    case "Log10":
                    case "Sin":
                    case "Tan":
                    case "Sqrt":
                    case "Sign":
                    case "Ceiling":
                    case "Floor":
                        this.Write(m.Method.Name.ToUpper());
                        this.Write("(");
                        this.Visit(m.Arguments[0]);
                        this.Write(")");
                        return m;
                    case "Atan2":
                        this.Write("ATN2(");
                        this.Visit(m.Arguments[0]);
                        this.Write(", ");
                        this.Visit(m.Arguments[1]);
                        this.Write(")");
                        return m;
                    case "Log":
                        if (m.Arguments.Count == 1)
                        {
                            goto case "Log10";
                        }
                        break;
                    case "Pow":
                        this.Write("POWER(");
                        this.Visit(m.Arguments[0]);
                        this.Write(", ");
                        this.Visit(m.Arguments[1]);
                        this.Write(")");
                        return m;
                    case "Round":
                        if (m.Arguments.Count == 1)
                        {
                            this.Write("ROUND(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", 0)");
                            return m;
                        }
                        else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                        {
                            this.Write("ROUND(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", ");
                            this.Visit(m.Arguments[1]);
                            this.Write(")");
                            return m;
                        }
                        break;
                    case "Truncate":
                        this.Write("ROUND(");
                        this.Visit(m.Arguments[0]);
                        this.Write(", 0, 1)");
                        return m;
                }
            }
            if (m.Method.Name == "ToString")
            {
                if (m.Object.Type != typeof(string))
                {
                    this.Write("CONVERT(NVARCHAR, ");
                    this.Visit(m.Object);
                    this.Write(")");
                }
                else
                {
                    this.Visit(m.Object);
                }
                return m;
            }
            else if (!m.Method.IsStatic && m.Method.Name == "CompareTo" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 1)
            {
                this.Write("(CASE WHEN ");
                this.Visit(m.Object);
                this.Write(" = ");
                this.Visit(m.Arguments[0]);
                this.Write(" THEN 0 WHEN ");
                this.Visit(m.Object);
                this.Write(" < ");
                this.Visit(m.Arguments[0]);
                this.Write(" THEN -1 ELSE 1 END)");
                return m;
            }
            else if (m.Method.IsStatic && m.Method.Name == "Compare" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 2)
            {
                this.Write("(CASE WHEN ");
                this.Visit(m.Arguments[0]);
                this.Write(" = ");
                this.Visit(m.Arguments[1]);
                this.Write(" THEN 0 WHEN ");
                this.Visit(m.Arguments[0]);
                this.Write(" < ");
                this.Visit(m.Arguments[1]);
                this.Write(" THEN -1 ELSE 1 END)");
                return m;
            }
            return base.VisitMethodCall(m);
        }

        protected void SetParameter(MethodCallExpression m)
        {
            object Value;
            FieldInfo field;
            if ((m.Arguments[0] as ConstantExpression) != null)
                Value = (m.Arguments[0] as ConstantExpression).Value;
            else
            {
                var b = (MemberExpression)m.Arguments[0];
                if ((b.Member as PropertyInfo) != null)
                {
                    PropertyInfo outerProp = (PropertyInfo)b.Member;
                    MemberExpression innerMember = (MemberExpression)b.Expression;
                    FieldInfo innerField = (FieldInfo)innerMember.Member;
                    ConstantExpression ce = (ConstantExpression)innerMember.Expression;
                    object innerObj = ce.Value;
                    object outerObj = innerField.GetValue(innerObj);
                    Value = (string)outerProp.GetValue(outerObj, null);
                }
                else
                {
                    field = (FieldInfo)(m.Arguments[0] as MemberExpression).Member;
                    Value = field.GetValue(((ConstantExpression)(m.Arguments[0] as MemberExpression).Expression).Value);
                }
            }


            AddParameter(new DBParameter
            {
                Name = parameterString,
                Type = m.Object.GetType().ToString(),
                Size = 100,
                ParameterDirection = ParameterDirection.Input,
                Value = Value
            });
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            // 2015-10-12 SQL NOT IN (use for List).
            //var m = (MethodCallExpression)u.Operand;
            //if (u.NodeType == ExpressionType.Not && m.Method.DeclaringType.Name == "List`1")
            //{
            //    switch (m.Method.Name)
            //    {
            //        case "Contains":
            //            this.Write("(");
            //            var position = m.Arguments[0].ToString().IndexOf(".") + 1;
            //            var length = m.Arguments[0].ToString().Length;
            //            this.Write(m.Arguments[0].ToString().Substring(position, length - position));
            //            this.Visit(m.Object);
            //            this.Write(" NOT in (" + inParameterString);
            //            this.Write(") )");
            //            inParameterString = string.Empty;
            //            return m;
            //    }
            //}

            // 其他的 NOT 條件判斷
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    // 2015-10-22 Create a private method for converting u to MethodCallExpression.
                    bool convertResult = ConvertMethodCallExpressopm(u);
                    if (convertResult)
                    {
                        var m = (MethodCallExpression)u.Operand;
                        if (u.NodeType == ExpressionType.Not && m.Method.DeclaringType.Name == "List`1")
                        {
                            switch (m.Method.Name)
                            {
                                case "Contains":
                                    this.Write("(");
                                    var position = m.Arguments[0].ToString().IndexOf(".") + 1;
                                    var length = m.Arguments[0].ToString().Length;
                                    this.Write(m.Arguments[0].ToString().Substring(position, length - position));
                                    this.Visit(m.Object);
                                    this.Write(" NOT in (" + inParameterString);
                                    this.Write(") )");
                                    inParameterString = string.Empty;
                                    return m;
                            }
                        }
                    }
                    else
                        sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        private bool ConvertMethodCallExpressopm(UnaryExpression u)
        {
            try
            {
                var m = (MethodCallExpression)u.Operand;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                sb.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        //sb.Append(((bool)c.Value) ? 1 : 0);
                        this.Write(parameterString);
                        AddParameter(new DBParameter
                        {
                            Name = parameterString,
                            Type = c.Type.ToString(),
                            Size = 100,
                            ParameterDirection = ParameterDirection.Input,
                            Value = c.Value
                        });
                        break;

                    case TypeCode.String:
                        //sb.Append("'");
                        //sb.Append(c.Value);
                        //sb.Append("'");
                        this.Write(parameterString);
                        AddParameter(new DBParameter
                        {
                            Name = parameterString,
                            Type = c.Type.ToString(),
                            Size = 100,
                            ParameterDirection = ParameterDirection.Input,
                            Value = c.Value
                        });
                        break;

                    case TypeCode.DateTime:
                        //sb.Append("'");
                        //sb.Append(c.Value);
                        //sb.Append("'");
                        this.Write(parameterString);
                        AddParameter(new DBParameter
                        {
                            Name = parameterString,
                            Type = c.Type.ToString(),
                            Size = 100,
                            ParameterDirection = ParameterDirection.Input,
                            Value = c.Value
                        });
                        break;

                    case TypeCode.Object:
                        //throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                        if (c.Value.GetType().Name == "List`1")
                        {
                            foreach (var VARIABLE in c.Value as List<string>)
                            {
                                parameterString = _Symbolizer + parameterCount.ToString();
                                inParameterString += _Symbolizer + parameterCount.ToString() + ",";
                                AddParameter(new DBParameter
                                {
                                    Name = parameterString,
                                    Type = c.Type.ToString(),
                                    Size = 100,
                                    ParameterDirection = ParameterDirection.Input,
                                    Value = VARIABLE
                                });
                                parameterCount++;
                            }
                            inParameterString = inParameterString.Substring(0, inParameterString.Length - 1);
                        }
                        break;
                    default:
                        //sb.Append(c.Value);
                        this.Write(parameterString);
                        AddParameter(new DBParameter
                        {
                            Name = parameterString,
                            Type = c.Type.ToString(),
                            Size = 100,
                            ParameterDirection = ParameterDirection.Input,
                            Value = c.Value
                        });
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                //parameterString = _Symbolizer + m.Member.Name;
                parameterString = _Symbolizer + parameterCount.ToString();
                parameterCount++;
                return m;
            }
            else
                if (m.Expression.NodeType == ExpressionType.Constant)
                {
                    var field = (FieldInfo)m.Member;
                    var value = field.GetValue(((ConstantExpression)m.Expression).Value);
                    var constantExpression = Expression.Constant(value);
                    this.VisitConstant(constantExpression);
                    return m;
                }
                else
                    if (m.Expression.NodeType == ExpressionType.MemberAccess)
                    {
                        PropertyInfo outerProp = (PropertyInfo)m.Member;
                        MemberExpression innerMember = (MemberExpression)m.Expression;
                        FieldInfo innerField = (FieldInfo)innerMember.Member;
                        ConstantExpression ce = (ConstantExpression)innerMember.Expression;
                        object innerObj = ce.Value;
                        object outerObj = innerField.GetValue(innerObj);
                        string value = (string)outerProp.GetValue(outerObj, null);
                        var constantExpression = Expression.Constant(value);
                        VisitConstant(constantExpression);
                        return m;
                    }
                    else
                        throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

        private bool ParseOrderByExpression(MethodCallExpression expression, string order)
        {
            UnaryExpression unary = (UnaryExpression)expression.Arguments[1];
            LambdaExpression lambdaExpression = (LambdaExpression)unary.Operand;

            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            MemberExpression body = lambdaExpression.Body as MemberExpression;
            if (body != null)
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    _orderBy = string.Format("{0} {1}", body.Member.Name, order);
                }
                else
                {
                    _orderBy = string.Format("{0}, {1} {2}", _orderBy, body.Member.Name, order);
                }

                return true;
            }

            return false;
        }

        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _take = size;
                return true;
            }

            return false;
        }

        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _skip = size;
                return true;
            }

            return false;
        }
    }

    public static class Evaluator
    {
        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
        }

        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }

        /// <summary>
        /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
        /// </summary>
        class SubtreeEvaluator : ExpressionVisitor
        {
            HashSet<Expression> candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return this.Visit(exp);
            }

            public override Expression Visit(Expression exp)
            {
                if (exp == null)
                {
                    return null;
                }
                if (this.candidates.Contains(exp))
                {
                    return this.Evaluate(exp);
                }
                return base.Visit(exp);
            }

            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }
                LambdaExpression lambda = Expression.Lambda(e);
                Delegate fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }
        }
    }

    class Nominator : ExpressionVisitor
    {
        Func<Expression, bool> fnCanBeEvaluated;
        HashSet<Expression> candidates;
        bool cannotBeEvaluated;

        internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
        {
            this.fnCanBeEvaluated = fnCanBeEvaluated;
        }

        internal HashSet<Expression> Nominate(Expression expression)
        {
            this.candidates = new HashSet<Expression>();
            this.Visit(expression);
            return this.candidates;
        }

        public override Expression Visit(Expression expression)
        {
            if (expression != null)
            {
                bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                this.cannotBeEvaluated = false;
                base.Visit(expression);
                if (!this.cannotBeEvaluated)
                {
                    if (this.fnCanBeEvaluated(expression))
                    {
                        this.candidates.Add(expression);
                    }
                    else
                    {
                        this.cannotBeEvaluated = true;
                    }
                }
                this.cannotBeEvaluated |= saveCannotBeEvaluated;
            }
            return expression;
        }
    }
}
