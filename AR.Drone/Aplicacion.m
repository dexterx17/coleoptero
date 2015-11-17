function varargout = Aplicacion(varargin)
% APLICACION M-file for Aplicacion.fig
%      APLICACION, by itself, creates a new APLICACION or raises the existing
%      singleton*.
%
%      H = APLICACION returns the handle to a new APLICACION or the handle to
%      the existing singleton*.
%
%      APLICACION('CALLBACK',hObject,eventData,handles,...) calls the local
%      function named CALLBACK in APLICACION.M with the given input arguments.
%
%      APLICACION('Property','Value',...) creates a new APLICACION or raises the
%      existing singleton*.  Starting from the left, property value pairs are
%      applied to the GUI before Aplicacion_OpeningFcn gets called.  An
%      unrecognized property name or invalid value makes property application
%      stop.  All inputs are passed to Aplicacion_OpeningFcn via varargin.
%
%      *See GUI Options on GUIDE's Tools menu.  Choose "GUI allows only one
%      instance to run (singleton)".
%
% See also: GUIDE, GUIDATA, GUIHANDLES

% Edit the above text to modify the response to help Aplicacion

% Last Modified by GUIDE v2.5 24-May-2012 21:42:00
% Begin initialization code - DO NOT EDIT

gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @Aplicacion_OpeningFcn, ...
                   'gui_OutputFcn',  @Aplicacion_OutputFcn, ...
                   'gui_LayoutFcn',  [] , ...
                   'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end
% End initialization code - DO NOT EDIT


% --- Executes just before Aplicacion is made visible.
function Aplicacion_OpeningFcn(hObject, eventdata, handles, varargin)
% This function has no output args, see OutputFcn.
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
% varargin   command line arguments to Aplicacion (see VARARGIN)

[x,map]=imread('ar2.jpg','jpg');
image(x),colormap(map),axis off,hold on;
set(handles.text1, 'String', 'OCTAVO INDUSTRIAL');
set(handles.text2, 'String', 'PROYECTO DE ROBOTICA');
set(handles.text3, 'String', 'DATOS:');
set(handles.text4, 'String', 'BATERIA (%):');
set(handles.text5, 'String', 'ANGULO X (rad):');
set(handles.text6, 'String', 'ANGULO Y (rad):');
set(handles.text7, 'String', 'ANGULO Z (rad):');
set(handles.text8, 'String', 'ALTURA (m):');
set(handles.text9, 'String', 'VELOCIDAD X (m/s):');
set(handles.text10, 'String', 'VELOCIDAD Y (m/s):');
set(handles.text11, 'String', 'VELOCIDAD Z (m/s):');
set(handles.text12, 'String', 'ACELERACI�N X (m/s2):');
set(handles.text13, 'String', 'ACELERACI�N Y (m/s2):');
set(handles.text14, 'String', 'ACELERACI�N Z (m/s2):');

% Choose default command line output for Aplicacion
handles.output = 0;

% Update handles structure
guidata(hObject, handles);

% UIWAIT makes Aplicacion wait for user response (see UIRESUME)
% uiwait(handles.figure1);


% --- Outputs from this function are returned to the command line.
function varargout = Aplicacion_OutputFcn(hObject, eventdata, handles) 
% varargout  cell array for returning output args (see VARARGOUT);
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Get default command line output from handles structure
varargout{1} = handles.output;

% --- Executes on button press in pushbutton1.
function pushbutton1_Callback(hObject, eventdata, handles)
X=ArDrone;
%abre el ejecutable%
    set(handles.text16, 'String', X(1,1)*100);
    set(handles.text17, 'String', X(2,1)*10);
    set(handles.text18, 'String', X(3,1)*10);
    set(handles.text19, 'String', X(4,1)*10); 
    set(handles.text20, 'String', X(5,1)*10);
    set(handles.text21, 'String', X(6,1));
    set(handles.text22, 'String', X(7,1));
    set(handles.text23, 'String', X(8,1));
    set(handles.text24, 'String', X(12,1));
    set(handles.text25, 'String', X(13,1));
    set(handles.text26, 'String', X(14,1));
    guidata(hObject,handles);
% --- Executes on button press in pushbutton3.
function pushbutton3_Callback(hObject, eventdata, handles)
% hObject    handle to pushbutton3 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
close all;


% --- Executes on button press in pushbutton4.
function pushbutton4_Callback(hObject, eventdata, handles)
% hObject    handle to pushbutton4 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
controles

% --- Executes on button press in pushbutton5.
function pushbutton5_Callback(hObject, eventdata, handles)
% hObject    handle to pushbutton5 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
trayectorias

% --- Executes on button press in pushbutton6.


% --- Executes on button press in pushbutton15.
function pushbutton15_Callback(hObject, eventdata, handles)
% hObject    handle to pushbutton15 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
winopen('ArDroneExample.exe')

