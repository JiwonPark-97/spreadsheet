﻿// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      27-Jan-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This file contains a single class DependencyGraph that represents the relationships between dependents and dependees.
/// </summary>
/// 
namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;
        private int size;


        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.ContainsKey(s))
                {
                    return dependees[s].Count;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<string> deps = dependents[s];
                if (deps.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                HashSet<string> dees = dependees[s];
                if (dees.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> returnSet = new HashSet<string>();
            if (HasDependents(s))
            {
                returnSet = dependents[s];
            }
            return returnSet;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> returnSet = new HashSet<string>();
            if (HasDependees(s))
            {
                returnSet = dependees[s];
            }
            return returnSet;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            // dependents[s] = t;
            // dependees[t] = s;

            // if s has dependents
            if (dependents.ContainsKey(s))
            {

                // prevent sizing up when adding duplicate pair
                if (dependents[s].Add(t))
                {
                    size++;
                }

            // if s has no dependents
            }
            else
            {
                dependents.Add(s, new HashSet<string>() { t });
                size++;
            }

            // if t has dependees
            if (dependees.ContainsKey(t))
            {
                dependees[t].Add(s);

            // if t has no dependees
            }
            else
            {
                dependees.Add(t, new HashSet<string>() { s });
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S </param>
        /// <param name="t"> t cannot be evaluated until s is </param>
        public void RemoveDependency(string s, string t)
        {
            // remove the pair from dependents
            if (dependents.ContainsKey(s))
            {
                dependents[s].Remove(t);
                size--;
            }

            // remove the pair from dependees
            if (dependees.ContainsKey(t))
            {
                dependees[t].Remove(s);
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            IEnumerable<string> originalDependents = GetDependents(s);

            // remove all existing pairs
            foreach (string dept in originalDependents)
            {
                RemoveDependency(s, dept);
            }

            // add new pairs
            foreach (string dept in newDependents)
            {
                AddDependency(s, dept);
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            IEnumerable<string> originalDependees = GetDependees(s);

            // remove all existing pairs
            foreach (string dees in originalDependees)
            {
                RemoveDependency(dees, s);
            }

            // add new pairs
            foreach (string dees in newDependees)
            {
                AddDependency(dees, s);
            }

        }

    }

}
