function Helicopter_Parameters(scale,color1,color2)

% HELICOPTER_PARAMETERS 

if nargin < 1
    scale = 1;
    color1 = [0.8 0 0];
    color2 = [0 0.4 0];
end 

if nargin < 2
    color1 = [0.8 0 0];
    color2 = [0 0.4 0];
end 

if nargin < 3
    color2 = [0 0.4 0];
end 

global Helicopter

%% Phisical Parameters
% Full length approx 1.2m
% Cockpit
% Lateral
Helicopter.Cockpit{1} = [0.05 0.125 0.125 0.075 0.025 -0.025 -0.04; -0.05 -0.05 -0.05 -0.05 -0.03 -0.03 -0.05; -0.05 -0.05 -0.02 -0.02 0.05 0.05 -0.01]*scale;
Helicopter.CockpitColor(1,:) =color1*1.2;
Helicopter.Cockpit{2} = [0.05 0.125 0.125 0.075 0.025 -0.025 -0.04;  0.05  0.05  0.05  0.05  0.03  0.03  0.05; -0.05 -0.05 -0.02 -0.02 0.05 0.05 -0.01]*scale;
Helicopter.CockpitColor(2,:) = color1;
% Front
Helicopter.Cockpit{3} = [0.20 0.20 0.20 0.20; -0.01 0.01 0.01 -0.01; -0.01 -0.01 -0.03 -0.03]*scale;
Helicopter.CockpitColor(3,:) = color1;
% Front Lateral
Helicopter.Cockpit{4} = [0.125 0.20 0.20 0.125; -0.05 -0.01 -0.01 -0.05; -0.05 -0.03 -0.01 -0.02]*scale;
Helicopter.CockpitColor(4,:) = color1;
Helicopter.Cockpit{5} = [0.125 0.20 0.20 0.125;  0.05  0.01  0.01  0.05; -0.05 -0.03 -0.01 -0.02]*scale;
Helicopter.CockpitColor(5,:) = color1;
% Front Upper 
Helicopter.Cockpit{6} = [0.025 0.075 0.125 0.20; -0.03 -0.05 -0.05 -0.01; 0.05 -0.02 -0.02 -0.01]*scale;
Helicopter.CockpitColor(6,:) = [0.4 0.4 0.4];
Helicopter.Cockpit{7} = [0.025 0.075 0.125 0.20;  0.03  0.05  0.05  0.01; 0.05 -0.02 -0.02 -0.01]*scale;
Helicopter.CockpitColor(7,:) = [0.4 0.4 0.4];
Helicopter.Cockpit{8} = [0.20 0.20 0.025 0.025; -0.01 0.01 0.03 -0.03; -0.01 -0.01 0.05 0.05]*scale;
Helicopter.CockpitColor(8,:) = [0.4 0.4 0.4];
% Front Bottom
Helicopter.Cockpit{9} = [0.125 0.20 0.20 0.125; -0.05 -0.01 0.01 0.05; -0.05 -0.03 -0.03 -0.05]*scale;
Helicopter.CockpitColor(9,:) = color1;
Helicopter.Cockpit{10} = [0.125 0.125 -0.05 -0.05; -0.05 0.05 0.05 -0.05; -0.05 -0.05 -0.05 -0.05]*scale;
Helicopter.CockpitColor(10,:) = color1;
Helicopter.Cockpit{11} = [0.05 0.05 -0.04 -0.04; -0.05 0.05 0.05 -0.05; -0.05 -0.05 -0.01 -0.01]*scale;
Helicopter.CockpitColor(11,:) = color1;
% Back
Helicopter.Cockpit{12} = [-0.10 -0.10 -0.10 -0.10; -0.01 0.01 0.01 -0.01; 0.01 0.01 0.03 0.03]*scale;
Helicopter.CockpitColor(12,:) = color2;
% Back Lateral
Helicopter.Cockpit{13} = [-0.04 -0.025 -0.10 -0.10; -0.05 -0.03 -0.01 -0.01; -0.01 0.05 0.03 0.01]*scale;
Helicopter.CockpitColor(13,:) = color2;
Helicopter.Cockpit{14} = [-0.04 -0.025 -0.10 -0.10;  0.05  0.03  0.01  0.01; -0.01 0.05 0.03 0.01]*scale;
Helicopter.CockpitColor(14,:) = color2;
% Back Upper
Helicopter.Cockpit{15} = [-0.025 0.025 0.025 -0.025; -0.03 -0.03 0.03 0.03; 0.05 0.05 0.05 0.05]*scale;
Helicopter.CockpitColor(15,:) = color2;

N_F = 6;
Thick = 0.007; % Thickness
t = linspace(0,2*pi,N_F+1);

xt = Thick*cos(t);
yt = Thick*sin(t);

zti = ones(size(t))*0.08;
zts = ones(size(t))*0.05;

for i = 1:N_F
    Helicopter.Cockpit{15+i} = [xt(i) xt(i) xt(i+1) xt(i+1); yt(i) yt(i) yt(i+1) yt(i+1); zti(i) zts(i) zts(i) zti(i)]*scale;
    Helicopter.CockpitColor(15+i,:) = color2;
end

Helicopter.Cockpit{16+N_F} = [-0.025 -0.025 -0.10 -0.10; -0.03 0.03 0.01 -0.01; 0.05 0.05 0.03 0.03]*scale;
Helicopter.CockpitColor(16+N_F,:) = color2;
% Back Bottom
Helicopter.Cockpit{17+N_F} = [-0.04 -0.04 -0.10 -0.10; -0.05 0.05 0.01 -0.01; -0.01 -0.01 0.01 0.01]*scale;
Helicopter.CockpitColor(17+N_F,:) = color1;

%% Tail 
N_F = 6;
Thick = 0.005; % Thickness
t = linspace(0,2*pi,N_F+1);

xti = -ones(size(t))*0.40;
xts = -ones(size(t))*0.10;

yt = Thick*cos(t);
zt = Thick*sin(t)+0.02;

% Right
for i = 1:N_F
    Helicopter.Tail{i} = [xti(i) xts(i) xts(i) xti(i); yt(i) yt(i) yt(i+1) yt(i+1); zt(i) zt(i) zt(i+1) zt(i+1)]*scale;
    Helicopter.TailColor(i,:) = color2;
end
Helicopter.Tail{N_F+1} = [-0.42 -0.40 -0.37 -0.39 -0.41 -0.40; 0.008 0.008 0.008 0.008 0.008 0.008; -0.04 -0.04 0.02 0.06 0.06 0.02]*scale;
Helicopter.TailColor(N_F+1,:) = [0.3 0.3 0.3];


%% Rotor
N_R = 12;
Thick = 0.05; % Thickness
t = linspace(0,2*pi,N_R+1);

xt = Thick*cos(t)-0.40;
yt = -ones(size(t))*0.008;
zt = Thick*sin(t)+0.02;

% Tail Rotor
Helicopter.Rotor{1} = [xt; yt; zt]*scale;
Helicopter.RotorColor(1,:) = [0.8 0.8 0.8];

% Main Rotor
Thick = 0.32; % Thickness

xt = Thick*cos(t);
yt = Thick*sin(t);
zt = ones(size(t))*0.08;

Helicopter.Rotor{2} = [xt; yt; zt]*scale;
Helicopter.RotorColor(2,:) = [0.8 0.8 0.8];

%% Base for landing
N_R = 12;
t = linspace(pi/4,3*pi/4,N_R+1);
xbfi = 0.07*ones(size(t));
xbfs = 0.08*ones(size(t));

xbbi = -0.03*ones(size(t));
xbbs = -0.04*ones(size(t));

yb =  0.1*sin(2*t);
zb =  0.05*(sin(t) - 1) - 0.05;

% Frontal base
for i = 1:N_R
    Helicopter.Base{i} = [xbfi(i) xbfi(i) xbfs(i) xbfs(i); yb(i) yb(i+1) yb(i+1) yb(i); zb(i) zb(i+1) zb(i+1) zb(i)]*scale;
    Helicopter.Base{N_R+i} = [xbbi(i) xbbi(i) xbbs(i) xbbs(i); yb(i) yb(i+1) yb(i+1) yb(i); zb(i) zb(i+1) zb(i+1) zb(i)]*scale;
    Helicopter.BaseColor(i,:) = [0.3 0.3 0.3];
    Helicopter.BaseColor(N_R+i,:) = [0.3 0.3 0.3];
end

N_F = 6;
Thick = 0.002; % Thickness
t = linspace(0,2*pi,N_F+1);

xti = -ones(size(t))*0.06;
xts =  ones(size(t))*0.10;

ytr = Thick*cos(t) + yb(1);
ytl = Thick*cos(t) + yb(end);

ztr = Thick*sin(t) + zb(1);
ztl = Thick*sin(t) + zb(end);

tam = length(Helicopter.Base);

for i = 1:N_F
    Helicopter.Base{tam+i} = [xti(i) xts(i) xts(i) xti(i); ytr(i) ytr(i) ytr(i+1) ytr(i+1); ztr(i) ztr(i) ztr(i+1) ztr(i+1)]*scale;
    Helicopter.BaseColor(tam+i,:) = color2;
    Helicopter.Base{tam+N_F+i} = [xti(i) xts(i) xts(i) xti(i); ytl(i) ytl(i) ytl(i+1) ytl(i+1); ztl(i) ztl(i) ztl(i+1) ztl(i+1)]*scale;
    Helicopter.BaseColor(tam+N_F+i,:) = color2;
end
