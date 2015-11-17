function Helicopter_Graph = Helicopter_Plot_3D(X,face)

% X = [x y z psi theta phi dx dy dz dpsi dtheta dphi]^T 

global Helicopter

if nargin < 2
    face = 1;
end

%%% Matriz de Rotacao
RotX = [1 0 0; 0 cos(X(4)) -sin(X(4)); 0 sin(X(4)) cos(X(4))];
RotY = [cos(X(5)) 0 sin(X(5)); 0 1 0; -sin(X(5)) 0 cos(X(5))];
RotZ = [cos(X(6)) -sin(X(6)) 0; sin(X(6)) cos(X(6)) 0; 0 0 1];

Rot = RotZ*RotY*RotX;

% Cockpit
for i = 1:length(Helicopter.Cockpit)
    rHelicopter = Rot * Helicopter.Cockpit{i};
    Helicopter_Graph(i) = patch(rHelicopter(1,:)+X(1),rHelicopter(2,:)+X(2),rHelicopter(3,:)+X(3),Helicopter.CockpitColor(i,:),'FaceAlpha',face);
end

tam = i;

% Tail
for i = 1:length(Helicopter.Tail)
    rHelicopter = Rot * Helicopter.Tail{i};    
    Helicopter_Graph(tam+i) = patch(rHelicopter(1,:)+X(1),rHelicopter(2,:)+X(2),rHelicopter(3,:)+X(3),Helicopter.TailColor(i,:));
end

tam = tam + i;

% Main and Tail Rotor
for i = 1:length(Helicopter.Rotor)
    rHelicopter = Rot * Helicopter.Rotor{i};    
    Helicopter_Graph(tam+i) = patch(rHelicopter(1,:)+X(1),rHelicopter(2,:)+X(2),rHelicopter(3,:)+X(3),Helicopter.RotorColor(i,:),'FaceAlpha',0.2);
end

tam = tam + i;

% Base for landing
for i = 1:length(Helicopter.Base)
    rHelicopter = Rot * Helicopter.Base{i};    
    Helicopter_Graph(tam+i) = patch(rHelicopter(1,:)+X(1),rHelicopter(2,:)+X(2),rHelicopter(3,:)+X(3),Helicopter.BaseColor(i,:),'FaceAlpha',0.2);
end

