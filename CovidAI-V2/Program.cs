using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidAI_V2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * HOW THIS CODE WORKS - A QUICK SUMMARY
             * ==========================================================================================================================
             * = An initial STATE is created of 0, the goal of the AI is to find which ACTIONS will change the STATE to the GOAL STATE  =
             * = The availabe actions and goal state is defined in the PROBLEM which is fed into the search                             =
             * ==========================================================================================================================
             * 
             * WHAT NEEDS TO BE DONE
             * +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
             * + {MED} Creat new searches by copying BFS and changing the search logic only (might need to change something else not 100% sure)    +
             * + {HRD} Centralise code such as trace - else there will be a lot of repeated code between each search                               +
             * + {HRD} make a better tree print since the currrent one is really hard to read - not really important though? only trace important  +
             * + {MED} Read in data setup and use the values within it to create actions                                                           +
             * + {EZY} Add a secondary cost that will be considered in searches (Economy effect / Sanity )                                         +
             * + {EZY} Create the final Covid problem object and run it                                                                            +
             * +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
             */
            
            //These actions are created with values (Change that they cause to state +value, ID)
            Action a1 = new Action(0, 0);
            Action a2 = new Action(1, 1);
            Action a3 = new Action(2, 2);
            Action a4 = new Action(3, 3);
            Action a5 = new Action(4, 4);
            Action a6 = new Action(5, 5);
            Action a7 = new Action(6, 6);
            Action a8 = new Action(7, 7);
            Action a9 = new Action(8, 8);
            Action a10 = new Action(9, 9);
            Action a11 = new Action(10, 10);
            Action a12 = new Action(11, 11);
            Action a13 = new Action(12, 12);
            Action a14 = new Action(13, 13);
            Action a15 = new Action(14, 14);
            Action a16 = new Action(15, 15);
            Action a17 = new Action(16, 16);
            Action a18 = new Action(17, 17);
            Action a19 = new Action(18, 18);
            Action a20 = new Action(19, 19);

            //Action set
            Action[] set = { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20 };

            //Initial set with defined initial state and set of actions possible
            State initS = new State(0, set);

            //The created problem consisting of the initial state and goal state value
            Problem p1 = new Problem(initS, 25);

            //Create the search with the problem to solve
            BreathFirstSearch br = new BreathFirstSearch(p1);

            //!!!!!!!! The code above sets up a simple problem to solve this is what will have to be changed to apply to covid searching once everything is done !!!!!!!!

            //UI stuff
            Console.WriteLine("<(- UNINFORMED COVID SEARCH AI -)>");
            Console.WriteLine("> The action IDs that lead to the final state through BFS as defined in the code are:");
            Console.WriteLine(br.trace);
        }

        //The problem class defines what needs to be found and what actions can be taken by the AI (problem definition)
        public partial class Problem
        {
            public State initialState; //The initial state 
            public int goal; //The final States value 

            public Problem(State inState, int inGoal)
            {
                initialState = inState;
                goal = inGoal;
            }

        }

        //The State aka Node is the result of an action being applied, this is what the tree is made of 
        public partial class State
        {
            public int Value; //What is it's value - this is what's checked later on to see if the goal has been found
            public Action[] actionSet; //What actions can be done on this state to get the next state 
            public State prevState;
            public int prevAC = 0; //ID of the previous action
           
            //Constructor 1 for the State - only used by initial state 
            public State(int envValue, Action[] validActions)
            {
                actionSet = new Action[validActions.Length]; 
                for (int i = 0; i < validActions.Length; i++)
                {
                    actionSet[i] = validActions[i];
                }

                Value = envValue;
            }

            //Constructor 2 of the State - same as 1 except also assigns the previous state and the action that was applied to it which lead to this state. 
            public State(int envValue, Action[] validActions, State inPrev, int inprevAC)
            {
                actionSet = new Action[validActions.Length];
                for (int i = 0; i < validActions.Length; i++)
                {
                    actionSet[i] = validActions[i];
                }

                Value = envValue;

                prevState = inPrev;

                prevAC = inprevAC;
            }

        }

        //This is what is applied to States to turn them into other States
        public partial class Action
        {
            public int Cost = 0; //What is the effect on the state
            int id = 0;
            public Action(int newCost, int newid)
            {
                Cost = newCost;
                id = newid;
            }

        }

        //Breath First search - takes in a problem and solves it 
        public class BreathFirstSearch
        {
            Queue<State> frontier;
            int goal; //value of the goal state

            //*** Used for printing UI - not important to searching ***
            int count = 0; //counts how many states where added to frontier 
            double linelength = 1; //how long the line should be before a space - is (used in number of actions) ^ lines 
            int uppercount = 1;//How many lines have been printed so far
            public String print;//The entire state tree
            public String trace;//The trace of every action ID taken that leads to found goal state

            public State final;//The found goal state - kinda useless legacy variable but still vital for calculation

            public BreathFirstSearch(Problem p)
            {
                frontier = new Queue<State>(); //Initialize the frontier
                goal = p.goal; //read in goal from problem
                frontier.Enqueue(p.initialState); //Add the intial state to frontier
                print = p.initialState.Value + " "; //UI thing - prints first state
                final = SearchFrontier(); //Start searching 
            }

            //Expand frontier by applying actions to state and adding the resultant states to queue 
            public void ExpandStateFrontier(State s)
            {
                //for every possible action
                for (int i = 0; i < s.actionSet.Length; i++)
                {
                    State rs = new State(s.Value + s.actionSet[i].Cost, s.actionSet, s, i);//apply action i to state to get resultant state rs
                    frontier.Enqueue(rs);//add rs to queue

                    count++;
                    if (count == linelength)//if maximum values have been printed / all children of previous on current line
                    {
                        print += "\n";//start a new line                              

                        linelength = Math.Pow(s.actionSet.Length, uppercount);//calculate how long the next line should be by calculating max children 
                        uppercount++;

                        count = 0;
                    }

                    //print state values in tree 
                    print += " - ";
                    print += rs.Value;
                }

            }

            //Compare State to goal State - This is the ?only? fuction that will be changed for diffrent types of searches 
            public State SearchFrontier()
            {
                while (frontier.Peek() != null)//While frontier is not empty
                {
                    State s = frontier.Dequeue();//Get the state from frontier queue 
                    if (s.Value == goal)
                    {
                        getTrace(s);//get trace to this node - which is the goal
                        return s;
                    }
                    ExpandStateFrontier(s);//expand nodes children and add to frontier 
                }
                return SearchFrontier();//keep on searching recursively 
            }

            //Get's the trace of every action that lead to a state s
            public void getTrace(State s)
            {
                //if previous action is not 0 - can cause errors here later on beacuse can never trace through id 0 state - not a problem for current test search but will mess up nothing state - easy fix 
                if(s.prevAC != 0)
                {
                    trace += " ==> " + s.prevAC;//write action that created this state to trace string 
                    getTrace(s.prevState);//recursive - move back through tree repeating ^ for that state 
                }
            }
        }
    }
}

