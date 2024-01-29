# Requirements Workup
### Tags on Events
## Elicitation

1. Is the goal or outcome well defined?  Does it make sense?

        The goal is to allow every event to have one or more tags to describe it or activities at the event. These tags will allow for additional functionality when filtering and fetching events, as well as allowing for user preferences and algorithms.

2. What is not clear from the given description?

        The tags are vague, as they are defined based on what the external APIs will provide.

3. How about scope?  Is it clear what is included and what isn't?

        The tags functionality is project-spanning in scope, but a fairly small footprint and mostly a read-only piece of information.

4. What do you not understand?
    * Technical domain knowledge

            What sort of tags will the API give us? Are they in a common format without duplicates? What happens if the event has no tags?

    * Business domain knowledge

            None

5. Is there something missing?

        Nothing that comes to mind.

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

