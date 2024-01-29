# Requirements Workup
### Tinder-like Cards
## Elicitation

1. Is the goal or outcome well defined?  Does it make sense?

        The goal is to allow the user to define their interests using a "yes or no" approach. If a user finds the event interesting, they can swip to the right; if not, they can swip to the left. This will tell Pop & Go which categories you like more or less than others and allow for affecting how many events of a category will be mixed into results.

2. What is not clear from the given description?

        From the description, the feature's operation is clear and concise. Potential questions of implementation would be few. Of note, will there be a limit to the number of random events in a given period? If so, what limit and over what period of time? Other sites have been infinite or shown up to 100 of a choice over the course of a day.

3. How about scope?  Is it clear what is included and what isn't?

        The scope is specifically a single page on the site. Its sole responsibility will be to show one event after another to the user, allowing the user to proceed at their own pace and stop when they wish. It will communicate the decision the user makes to the site and fetch another event to display in the cycle.

4. What do you not understand?
    * Technical domain knowledge

            How can you process a dragging event to move the card? How can we animate or show the card's movement to the user? Can this be accessibility-friendly (how would this differ for screen-readers? Give them a dialog choice?)?

    * Business domain knowledge

            How will the results affect the algorithm? What sort of scoring system are we using? Can the algorithm be efficient/performant?

5. Is there something missing?

        Nothing comes to mind right away.

6. Get answers to these questions.

## Analysis

Go through all the information gathered during the previous round of elicitation.  

1. For each attribute, term, entity, relationship, activity ... precisely determine its bounds, limitations, types and constraints in both form and function.  Write them down.
2. Do they work together or are there some conflicting requirements, specifications or behaviors?
3. Have you discovered if something is missing?  
4. Return to Elicitation activities if unanswered questions remain.


## Design and Modeling
Our first goal is to create a **data model** that will support the initial requirements.

1. Identify all entities;  for each entity, label its attributes; include concrete types
2. Identify relationships between entities.  Write them out in English descriptions.
3. Draw these entities and relationships in an _informal_ Entity-Relation Diagram.
4. If you have questions about something, return to elicitation and analysis before returning here.

## Analysis of the Design
The next step is to determine how well this design meets the requirements _and_ fits into the existing system.

1. Does it support all requirements/features/behaviors?
    * For each requirement, go through the steps to fulfill it.  Can it be done?  Correctly?  Easily?
2. Does it meet all non-functional requirements?
    * May need to look up specifications of systems, components, etc. to evaluate this.

