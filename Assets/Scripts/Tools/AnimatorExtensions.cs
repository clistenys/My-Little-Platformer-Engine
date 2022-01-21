using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions
{
    /// <summary>
    /// Triggers an animator trigger.
    /// </summary>
    /// <param name="animator">Animator.</param>
    /// <param name="parameter">Parameter name.</param>
    /// <param name="value">If set to <c>true</c> value.</param>
    public static bool SetAnimatorTrigger(Animator animator, int parameter, HashSet<int> parameterList, bool performSanityCheck = true)
    {
        if (performSanityCheck && !parameterList.Contains(parameter))
        {
            return false;
        }
        animator.SetTrigger(parameter);
        return true;
    }

    /// <summary>
    /// Triggers an animator trigger.
    /// </summary>
    /// <param name="animator">Animator.</param>
    /// <param name="parameterName">Parameter name.</param>
    /// <param name="value">If set to <c>true</c> value.</param>
    public static void SetAnimatorTrigger(Animator animator, string parameterName, HashSet<string> parameterList, bool performSanityCheck = true)
    {
        if (parameterList.Contains(parameterName))
        {
            animator.SetTrigger(parameterName);
        }
    }
}
