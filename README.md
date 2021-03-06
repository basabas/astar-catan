# AStar Catan

Unity version: 2019.4.15f  
Made for standalone  


### Why does this repository exist
This repository is meant to showcase a technical assignment I had to do for a company. I went a bit overboard and added some cool stuff to flex a bit.

### Things I did in this assignment
* Drawing is done using an instanced shader. This should make the application be able to 
draw huge amounts of nodes. 
* The shader has a TextureArray with all five nodeTextures that
were provided. You can make your own arrays by going to Tools>Create TextureArray. The 
drawing of the shader is done in WorldDrawer.cs.
* For the Neighbours property in IAStarNode, I created a new Ienumerable<IAStarNode> 
(NeighbourNodeCollection) that uses a 2D array of IAStarNodes and an index to iterate 
through all neighbors of a provided index in that array.
*  Input is done using the new InputSystem. I have almost no knowledge of this system but it 
has events, so I hooked up the InputHandler class to a mousebutton event.
* I have created the option to create and select different worlds. You can set node types, 
travel costs, world size(gridsize basically) and camera orientation. Nodes that should be 
ignored(water) will need to have its travel cost set to a negative number. I opted for a 
boolean but decided this was more than enough. 
  * You can create a world by rightclicking in the project assetsfolder and than 
Create>Bas>New WorldSettings.
  * You can select any of your created worlds from a Dropdown when running the 
application. This Dropdown fills itself with all created world from the Resources>Worlds 
folder.
  * The WorldInformation has a custom inspector to easily adjust the nodes list.
* The EstimatedCostTo method of IAStarNode should return the amount of steps the node is 
away from the given goal.
*  I created my own AStar class to see if I could improve on the provided Astar class. You can 
select a toggle (Improved A*) to make use of the improvements. It uses a Heap for optimal 
insert/sorting/removing from the open set and a HashSet for the closed set


### Potential ideas for later
* Have the time it takes to calculate the fastest path be displayed in the UI rather than console.
* Animations of the nodes when selecting them and for displaying the path after calculations.
* Async for the calculations
