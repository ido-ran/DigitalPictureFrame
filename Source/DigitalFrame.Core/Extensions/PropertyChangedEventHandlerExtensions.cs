using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace DigitalFrame.Core.Extensions
{
    public static class PropertyChangedEventHandlerExtensions
    {
        public static void NotifyPropertyChanged(this PropertyChangedEventHandler eventHandler, Expression<Func<object>> expression)
        {
            if (null == eventHandler)
            {
                return;
            }
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }
            var constantExpression = memberExpression.Expression as ConstantExpression;
            var propertyInfo = memberExpression.Member as PropertyInfo;

            foreach (var del in eventHandler.GetInvocationList())
            {
                try
                {
                    del.DynamicInvoke(new object[] { constantExpression.Value, new PropertyChangedEventArgs(propertyInfo.Name) });
                }
                catch (TargetInvocationException e)
                {

                }
            }
        }

    }
}
