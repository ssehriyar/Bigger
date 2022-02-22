# Game and Algorithm	Unity ver 2020.3.28f1

The game has 3 base attributes:

1-BoardSize is my board size also my level of difficulty. 
2-GameObjectPieceNumber is the piece number. It is selected randomly from a certain range according to the level of difficulty.
3-Seed is the base of randomness. In procedural generation and other game parts using random values. To be able to generate and keep my level information there are LevelArgs and RandomUtil classes.

GameManager starts the game with BoardSize and GameObjectPieceNumber. Firstly the game board is created. Then Procedural class starts.

Procedural generation starts with creating 4 triangles that shape a square. This event continues until the board is covered. After the board is covered the main piece objects will be created. Pieces will get a random position on board. The triangle in that position is going to be the first child of the piece.

At this point, I sampled the breadth-first search algorithm. All first child neighbors will be enqueued in Queue. After that, the queue loop is started until all triangles find a parent. The condition to get a parent is passing a threshold that is random. If the triangle can’t pass the threshold it will be enqueued.

If a triangle passes the threshold its parent will be the one who put him in the queue. Also it will enqueue its neighbors. So the whole algorithm is based on breadth-first search. 
