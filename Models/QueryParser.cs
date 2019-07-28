using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Player_Service.Models {
    public class QueryTokens {

        public QueryTokens (List<string> _operands, List<string> _operators, List<string> _clauses) {
            this.operands = operands;
            this.operators = _operators;
            this.clauses = _clauses;
        }
        public QueryTokens() {
            this.operands = new List<string>();
            this.operators = new List<string>();
            this.clauses = new List<string>();
        }
        public List<string> operands { get; set;}
        public List<string> operators { get; set;}
        public List<string> clauses { get; set;}
    }

    public class QueryParser {
        const string AND_CLAUSE = "and";
        const string OR_CLAUSE = "or";
        const string EQUALS = "=";
        const string GREATER_THAN = ">";
        const string LESS_THAN = "<";
        const string GREATER_THAN_EQUAL = ">=";
        const string LESS_THAN_EQUAL = "<=";

        string[] clausesArray = new string[] { AND_CLAUSE, OR_CLAUSE };
        string[] operatorsArray = new string[] { EQUALS, GREATER_THAN, LESS_THAN, GREATER_THAN_EQUAL, LESS_THAN_EQUAL };

        public QueryParser() {}

        private QueryTokens tokenizeQuery(string query) {
            string[] tokens = query.Split(" ");

            QueryTokens queryTokens = new QueryTokens();

            foreach (var item in tokens) {
                if (clausesArray.Contains(item)) {
                    queryTokens.clauses.Add(item);
                    continue;
                }
                if (operatorsArray.Contains(item)) {
                    queryTokens.operators.Add(item);
                    continue;
                }
                queryTokens.operands.Add(item);
            }
            int totalClauses = queryTokens.clauses.Count;
            int totalOperators = queryTokens.operators.Count;
            int totalOperands = queryTokens.operands.Count;

            return queryTokens;
        }

        public Expression<Func<User, bool>> buildQuery (string query) {

            QueryTokens tokens = this.tokenizeQuery(query);
            string[] operands = tokens.operands.ToArray();
            string[] operators = tokens.operators.ToArray();
            string[] clauses = tokens.clauses.ToArray();

            Expression finalExpression = null;
            int operatorIndex = 0;

            List<Expression> expressionList = new List<Expression>();
            var parameter = Expression.Parameter(typeof(User), "u");

            for (int i = 0; i < operands.Length; i += 2)
            {
                var memberExpression = Expression.Property(parameter, operands[i]);
                
                var isConstantNumeric = int.TryParse(operands[i+1], out int number);
                Expression constant = null;

                if (isConstantNumeric) {
                    constant = Expression.Constant(number, typeof(int));
                }
                else {
                    constant = Expression.Constant(operands[i + 1]);
                }

                BinaryExpression expressionBody = null;

                switch (operators[operatorIndex])
                {
                    case EQUALS:
                        
                        expressionBody = Expression.Equal(memberExpression, constant);
                        break;

                    case GREATER_THAN:
                        expressionBody = Expression.GreaterThan(memberExpression, constant);
                        break;

                    case GREATER_THAN_EQUAL:
                        expressionBody = Expression.GreaterThanOrEqual(memberExpression, constant);
                        break;

                    case LESS_THAN:
                        expressionBody = Expression.LessThan(memberExpression, constant);
                        break;

                    case LESS_THAN_EQUAL:
                        expressionBody = Expression.LessThanOrEqual(memberExpression, constant);
                        break;

                    default:
                        Console.WriteLine("Undefined condition encountered. Malformed query");
                        break;
                }
                expressionList.Add(expressionBody);
                ++operatorIndex;
            }

            if (clauses.Length > 0) {
                for (int i = 0; i < clauses.Length; i++) {
                    switch (clauses[i]) {
                        case AND_CLAUSE:
                            if (finalExpression == null) {
                                finalExpression = Expression.AndAlso(expressionList[i], expressionList[i+1]);
                            }
                            else {
                                finalExpression = Expression.AndAlso(finalExpression, expressionList[i+1]);
                            }
                        break;

                        case OR_CLAUSE:
                            if (finalExpression == null) {
                                finalExpression = Expression.OrElse(expressionList[i], expressionList[i+1]);
                            }
                            else {
                                finalExpression = Expression.OrElse(finalExpression, expressionList[i+1]);
                            }
                        break;

                        default:
                            Console.WriteLine("Unwanted clause encountered. Malformed query");
                        break;
                    }
                }
                var userExpressionTree = Expression.Lambda<Func<User, bool>>(finalExpression, new[] {parameter});
                return userExpressionTree;
            }
            else {
                // No clause exists, thus we will return only one expression
                var userExpressionTree = Expression.Lambda<Func<User, bool>>(expressionList[0], new[] {parameter});
                return userExpressionTree;
            }
        }
    }
}