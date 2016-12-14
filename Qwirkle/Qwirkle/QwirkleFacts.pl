isSpace(X, Y, space(X, Y)).
isPlay(play(_,T), T).
xy(space(X, Y), X, Y).
xy(play(S, _), X, Y) :- xy(S, X, Y).

x(S, X) :- xy(S, X, _).
y(S, Y) :- xy(S, _, Y).

pc_b([[P | C] | B], P, C, B).
pcb([[P | C] | B], P, [C | B]).

p(B, P) :- pcb(B, P, _).
cb(B, CB) :- pcb(B, _, CB).
b([_ | B], B).

pb(B, P, B2) :- pc_b(B, P, _, B2).

delta(D, S, T, N) :-
	call(D, S, C1),
	call(D, T, C2),
	N is C1 - C2.
	
spaces(Xmin, Xmax, Ymin, Ymax, space(X, Y)) :-
	between(Xmin, Xmax, X),
	between(Ymin, Ymax, Y).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	
lookLeft(E, S, B, N) :- %column would not be empty
	p(B, P),
	!,
	delta(x, S, P, M),
	lookLeft(E, M, S, B, N).
	
lookLeft(E, S, B, N) :- %board is empty
	lookLeft(E, -1, S, B, N).

lookLeft(0, 0, S, B, N) :- %expected to find the actual column and found it
	!,
	lookUp(6, S, B, N).
	
lookLeft(0, _, S, B, N) :- %expected to find the actual coumn but it is not there
	% go to up and just find 6 blanks
	!,
	lookUp(6, S, [[]|B], N).
	
lookLeft(E, E, S, B, N) :-
	!,
	lookNearLeft(E, S, B, N).
	
lookLeft(E, M, S, B, N) :-
	M > E, %skip the column
	!,
	M2 is M - 1,
	b(B, B2),
	lookLeft(E, M2, S, B2, N).
	
lookLeft(E, M, S, B, [S2 | N]) :- %too close but on board
	xy(S, X, Y),
	X2 is X - E,
	X2 >= 0,
	!,
	E2 is E - 1,% this guy is out of order, but should be delayed until after the cut
	isSpace(X2, Y, S2),
	lookLeft(E2, M, S, B, N).

lookLeft(E, M, S, B, [[] | N]) :- %too close and not on board
	E2 is E - 1,
	lookLeft(E2, M, S, B, N).
	
lookNearLeft(0, S, B, N) :-
	!,
	lookUp(6, S, B, N).
	
lookNearLeft(E, S, [], N) :- %board is empty, but we need to pick left neighbors
	!,
	lookNearLeft(E, -1, S, [[]], N).
	
lookNearLeft(E, S, B, N) :- %we are in the correct column and we need to see how far down to look
	p(B, P),
	!,
	delta(y, S, P, M),
	lookNearLeft(E, M, S, B, N).%we have found the distance to the next point
	
lookNearLeft(E, S, B, N) :- %the column is empty
	lookNearLeft(E, -1, S, B, N).
	
lookNearLeft(E, 0, S, B, [P | N]) :-
	!,
	E2 is E - 1,
	pb(B, P, B2),
	lookLeft(E2, S, B2, N).
	
lookNearLeft(E, M, S, B, N) :-
	M > 0,
	!,
	cb(B, CB),
	lookNearLeft(E, S, CB, N).

lookNearLeft(E, _, S, B, [S2 | N]):- %we passed a left neightbor, time to put in a blank
	E2 is E - 1,
	b(B, B2),
	xy(S, X, Y),
	X2 is X - E,
	isSpace(X2, Y, S2),
	lookNearLeft(E2, S, B2, N).

lookUp(0, S, B, N) :- 
	!,
	lookCenter(S, B, N).
	
lookUp(E, S, B, N) :-
	p(B, P),
	!,
	delta(y, S, P, M),
	lookUp(E, M, S, B, N).
	
lookUp(E, S, B, N) :- %this means that p(B, _) fails
	lookUp(E, -1, S, B, N).
	
lookUp(0, _, S, B, N) :- 
	!,
	lookCenter(S, B, N).
	
lookUp(E, E, S, B, [P | N]) :-
	!,
	E2 is E - 1,%look 1 closer
	pcb(B, P, CB),
	lookUp(E2, S, CB, N).
	
lookUp(E, M, S, B, N) :-
	M > E, %skip play
	!,
	cb(B, CB),
	lookUp(E, S, CB, N).% don't know how close so we have to remeasure

lookUp(E, M, S, B, [S2 | N]):- %we passed a up neightbor, time to put in a blank
	xy(S, X, Y),
	Y2 is Y - E,
	Y2 >= 0,
	!,
	E2 is E - 1,
	isSpace(X, Y2, S2),
	lookUp(E2, M, S, B, N).

lookUp(E, M, S, B, [[] | N]) :- %too close and not on board
	E2 is E - 1,
	lookUp(E2, M, S, B, N).

lookCenter(S, B, N) :-
	p(B, P),
	xy(S, X, Y),
	not(xy(P, X, Y)),
	!,
	lookDown(1, S, B, N).
	
lookCenter(S, B, N) :-
	not(p(B, _)),
	lookDown(1, 7, S, B, N).
	
lookDown(E, S, B, N) :-
	p(B, P),
	!,
	delta(y, P, S, M),
	lookDown(E, M, S, B, N).

lookDown(E, S, B, N) :- %column is missing or blank
	lookDown(E, 7, S, B, N).

lookDown(7, _, S, B, N) :- 
	b(B, B2), %lose the column
	!,
	lookRight(1, S, B2, N).
	
lookDown(7, _, S, B, N) :- %board is empty
	!,
	lookRight(1, S, B, N).
	
lookDown(E, E, S, B, [P | N]) :-
	!,
	E2 is E + 1, %look a farther
	pcb(B, P, CB),
	lookDown(E2, S, CB, N).

lookDown(E, M, S, B, [S2 | N]) :- %measurement is gaurenteed to not be less than expected
	xy(S, X, Y),
	Y2 is Y + E,
	Y2 =< 29,
	!,
	E2 is E + 1,
	isSpace(X, Y2, S2),
	lookDown(E2, M, S, B, N).
	
lookDown(E, M, S, B, [[] | N]) :- %so this happens if the last down neighbors need to be []	
	E2 is E + 1,
	lookDown(E2, M, S, B, N).
	
lookRight(E, S, B, N) :-
	p(B, P),
	!,
	delta(x, P, S, M),
	lookRight(E, M, S, B, N).
	
lookRight(E, S, B, N) :-%p failed
	lookRight(E, 7, S, B, N).
	
lookRight(7, _, _, _, []) :- !. %done, bitches !

lookRight(E, E, S, B, N) :-
	!,
	lookNearRight(E, S, B, N).
	
lookRight(E, M, S, B, [S2 | N]) :-
	xy(S, X, Y),
	X2 is X + E,
	X2 =< 15,
	!,
	E2 is E + 1,
	isSpace(X2, Y, S2),
	lookRight(E2, M, S, B, N).

lookRight(E, M, S, B, [[] | N]) :-
	E2 is E + 1,
	lookRight(E2, M, S, B, N).
	
lookNearRight(E, S, B, N) :-
	p(B, P),
	!,% the column exists and is not empty
	delta(y, S, P, M),
	lookNearRight(E, M, S, B, N).

lookNearRight(E, S, B, N) :- % p fails	
	lookNearRight(E, -1, S, B, N).
	
lookNearRight(E, 0, S, B, [P | N]) :-
	!,
	E2 is E + 1,
	pb(B, P, B2),
	lookRight(E2, S, B2, N).

lookNearRight(E, M, S, B, N) :-
	M > 0,
	!,
	cb(B, CB),
	lookNearRight(E, S, CB, N).
	
lookNearRight(E, _, S, B, [S2 | N]) :-
	E2 is E + 1,
	b(B, B2),
	xy(S, X, Y),
	X2 is X + E,
	isSpace(X2, Y, S2),
	lookRight(E2, S, B2, N).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	
trimSpoke([P | L], [T | S]) :-
	isPlay(P, T),
	!,
	trimSpoke(L, S).
	
trimSpoke(_, []).	

isEmpty([]).

sum(A, B, C) :- C is A + B.

counts(TS, ShapeCount, ColorCount) :-
	maplist(shapeAndColor, TS, Shapes, Colors),%maps TS into two other lists
	foldl(distinct,Shapes, [], DShapes),
	foldl(distinct,Colors, [], DColors),
	length(DShapes, ShapeCount),
	length(DColors, ColorCount).

isLegal([]) :- !.

isLegal(TS) :-
	counts(TS, ShapeCount, ColorCount),
	length(TS, TileCount),
	isLegal(TileCount, ShapeCount, ColorCount).
	
isLegal(T, 1, T) :- !.

isLegal(T, T, 1).

shapeAndColor(tile(S, C), S, C).

shape(tile(S, _), S).
color(tile(_, C), C).

distinct(A, DAS, [A | DAS]) :-
	not(member(A, DAS)),
	!.
	
distinct(_, DAS, DAS).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	
isGap(S, B, [Left1, Up1, Down1, Right1], AllowedTiles) :-
	between(0, 15, X),
	between(0, 29, Y),
	S = space(X, Y),
	lookLeft(6, S, B, [	N1 , N2 , N3 , N4 , N5, N6,
						N7 , N8 , N9 , N10, N11, N12,
						N13, N14, N15, N16, N17, N18,
						N19, N20, N21, N22, N23, N24]),
	Left1	=	[N6 , N5 , N4 , N3 , N2 , N1],
	Up1		=	[N12, N11, N10, N9 , N8 , N7],
	Down1	=	[N13, N14, N15, N16, N17, N18],
	Right1	=	[N19, N20, N21, N22, N23, N24],
	trimSpoke(Left1, Left2),
	trimSpoke(Up1, Up2),
	trimSpoke(Down1, Down2),
	trimSpoke(Right1, Right2),
	Spokes2 = [Left2, Up2, Down2, Right2],
	exclude(isEmpty, Spokes2, Spokes3),
	not(isEmpty(Spokes3)),% not yonder
	append(Left2, Right2, Horizontal),
	isLegal(Horizontal),
	append(Up2, Down2, Vertical),
	isLegal(Vertical), %not dead space 1 because it touches a play
	maplist(allowedTiles, Spokes3, [FirstSpoke | RestOfThem]),
	foldl(intersection, RestOfThem, FirstSpoke, AllowedTiles),
	length(AllowedTiles, L),
	L > 0.
	 
allShapes([cross, clover, square, diamond, circle, star]).
allColors([red, orange, yellow, green, blue, purple]).
	 
allowedTiles(Spoke, AllowedTiles) :-
	counts(Spoke, ShapeCount, ColorCount),
	allowedTilesSameShape(Spoke, ShapeCount, ColorCount, SameShape),
	allowedTilesSameColor(Spoke, ShapeCount, ColorCount, SameColor),
	append(SameShape, SameColor, AllowedTiles).
	
allowedTilesSameShape([tile(S, C1) | Spokes], 1, _, SameShape) :-
	!,
	maplist(color, [tile(S, C1) | Spokes], Colors),
	allColors(AllColors),
	difference(AllColors, Colors, MissingColors),
	assert(curry(C2, tile(S, C2))), 
	maplist(curry, MissingColors, SameShape),
	retract(curry(_,_)).
	
allowedTilesSameShape(_,_,_,[]).
	
allowedTilesSameColor([tile(S1, C) | Spokes], _, 1, SameColor) :-
	!,
	maplist(shape, [tile(S1, C) | Spokes], Shapes),
	allShapes(AllShapes),
	difference(AllShapes, Shapes, MissingShapes),
	assert(curry(S2, tile(S2, C))), 
	maplist(curry, MissingShapes, SameColor),
	retract(curry(_,_)).
	
allowedTilesSameColor(_,_,_,[]).

intersection([X | XS], YS, [X | ZS]) :-
	member(X, YS),
	!,
	intersection(XS, YS, ZS).
	
intersection([_ | XS], YS, ZS) :-
	!,
	intersection(XS, YS, ZS).

intersection([], _, []) :- !.
intersection(_, [], []) :- !.	
	
difference([], _, []) :-!.
difference(S, [], S) :-!.
difference([X | XS], YS, [X | ZS]) :-
	not(member(X, YS)),
	!,
	difference(XS, YS, ZS).
	
difference([_ | XS], YS, ZS) :-
	difference(XS, YS, ZS).
	
gaps(B, Bag) :-
	findall(gap(Space, Spokes, AllowedTiles), isGap(Space, B, Spokes, AllowedTiles), Bag).
	
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%	
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

plays(B, H, P) :-
	gaps(B, Bag1),
	assert((intersectWithHand(gap(Space, Spokes, AllowedTiles), gap(Space, Spokes, Intersection)) :- intersection(H, AllowedTiles, Intersection))), 
	maplist(intersectWithHand, Bag1, Bag2),
	retractall(intersectWithHand(_,_)),
	include(matchesHand, Bag2, Bag3),
	%trace,
	makePlays(Bag3, P).
	%length(P, L).
	
makePlays([gap(Space, _, AllowedTiles)| Rest], [[play(S, T)]| P2]) :-
	getSpace(Space, S),
	getTile(AllowedTiles, T),
	makePlays(Rest, P2).
makePlays([], []).

getSpace(S, S).
getTile([T|Rest],T).
getTile([], _).


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
isLegalPlay(T) :- isSingleColumn(T), !, isLegalTile(T), !.
isLegalPlay(T) :- isSingleRow(T), !, isLegalTile(T), !. 
isLegalTile(T) :- isSingleShape(T), !, isLegalShape(T), !.
isLegalTile(T) :- isSingleColor(T), !, isLegalColor(T), !.

	
isLegalColor([play(space(_,_), tile(S, _)) | Rest]) :- 
		notContainsShape(Rest, S),
		!,
		isLegalColor(Rest).
isLegalColor([play(space(_,_), tile(S, _))]).
isLegalColor([]).

isLegalShape([play(space(_,_), tile(_, C)) | Rest]) :-
		notContainsColor(Rest, C),
		!,
		isLegalShape(Rest).
isLegalShape([play(space(_,_), tile(_, C))]).
isLegalShape([]).

		
/* Contains */
notContainsColor([play(space(_,_), tile(_, C))], Color) :-
		C \= Color.
notContainsColor([play(space(_,_), tile(_, C)) | Rest], Color) :- 
		C \= Color,
		!,
		notContainsColor(Rest, Color).

notContainsShape([play(space(_,_), tile(S, _))], Shape) :-
		S \= Shape.
notContainsShape([play(space(_,_), tile(S, _)) | Rest], Shape) :- 
		S \= Shape,
		!,
		notContainsShape(Rest, Shape).
		
/* Single Type */
isSingleColor([]).
isSingleColor([play(space(_,_), tile(_, _))]) :- !.
isSingleColor([play(space(_,_), tile(_, C1)), play(space(_,_), tile(_, C2))]) :- C1 == C2.
isSingleColor([play(space(_,_), tile(_, C1)), play(space(_,_), tile(S2, C2)) | Rest]) :- C1 == C2, !, isSingleColor([play(space(_,_), tile(S2,C2)) | Rest]).

isSingleShape([]).
isSingleShape([play(space(_,_), tile(_, _))]) :- !.
isSingleShape([play(space(_,_),tile(S1, _)), play(space(_,_),tile(S2, _))]) :- S1 == S2.
isSingleShape([play(space(_,_),tile(S1, _)), play(space(_,_),tile(S2, C2)) | Rest]) :- S1 == S2, !, isSingleShape([play(space(_,_),tile(S2,C2)) | Rest]).

isSingleRow([]).
isSingleRow([play(space(X1,_), tile(_, _))]) :- !.
isSingleRow([play(space(X1,_),tile(_, _)), play(space(X2,_),tile(_, _))]) :- X1 == X2.
isSingleRow([play(space(X1,_),tile(_, _)), play(space(X2,Y2),tile(_, _)) | Rest]) :- X1 == X2, !, isSingleRow([play(space(X2,Y2),tile(_,_)) | Rest]).

isSingleColumn([]).
isSingleColumn([play(space(_,Y1), tile(_, _))]) :- !.
isSingleColumn([play(space(_,Y1),tile(_, _)), play(space(_,Y2),tile(_, _))]) :- Y1 == Y2.
isSingleColumn([play(space(_,Y1),tile(_, _)), play(space(X2,Y2),tile(_, _)) | Rest]) :- Y1 == Y2, !, isSingleColumn([play(space(X2,Y2),tile(_,_)) | Rest]).


matchesHand(gap(_,_,Allowed)) :- length(Allowed, N), N > 0.

	
	sample(
	[[play(space(1,11), tile(cross,yellow))],
	[play(space(2,11), tile(cross,orange)),play(space(2,15), tile(circle,yellow))],
	[play(space(3,11), tile(cross,purple)),play(space(3,13), tile(clover,green)),play(space(3,15), tile(circle,blue))],
	[play(space(4,11), tile(cross,green)),play(space(4,13), tile(clover,purple)),play(space(4,15), tile(circle,green))],
	[play(space(5,10), tile(diamond,red)),play(space(5,11), tile(cross,red)),play(space(5,12), tile(star,red)),play(space(5,13), tile(clover,red)),play(space(5,15), tile(circle,red)),play(space(5,16), tile(star,red)),play(space(5,17), tile(cross,red))],
	[play(space(6,10), tile(diamond,blue)),play(space(6,12), tile(star,orange)),play(space(6,13), tile(clover,orange)),play(space(6,14), tile(square,orange)),play(space(6,15), tile(circle,orange)),play(space(6,17), tile(cross,green))],
	[play(space(7,11), tile(diamond,blue)),play(space(7,12), tile(star,blue)),play(space(7,14), tile(square,green))],
	[play(space(8,12), tile(star,green)),play(space(8,13), tile(cross,green)),play(space(8,17), tile(diamond,blue))],
	[play(space(9,13), tile(cross,orange)),play(space(9,14), tile(clover,orange)),play(space(9,15), tile(circle,orange)),play(space(9,16), tile(square,orange)),play(space(9,17), tile(diamond,orange))],
	[play(space(10,16), tile(square,green))],
	[play(space(11,16), tile(square,red)),play(space(11,17), tile(clover,red))]
]).

hand([tile(clover,orange), tile(clover,red), tile(circle,blue), tile(star, purple), tile(circle, blue), tile(circle,yellow)]).
