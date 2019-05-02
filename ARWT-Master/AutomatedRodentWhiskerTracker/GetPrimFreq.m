
function f = GetPrimFreq(theta)

% parameters
T = 1/500;
fq_nyquist = 0.5/T;
fq_min = 4;
fq_max = 16;
n_fq_max = round((1/fq_max) / T);
n_smooth = round(n_fq_max / 4);
type = 'linear';
windowSize = 5;
% % pre-filter signal
% [b, a] = butter(3, (fq_max*2)/fq_nyquist);
% theta = filter(b, a, theta);
% theta = theta + m;

% pre-filter signal

theta = smooth(theta,5)

% derived
N = length(theta);
if size(theta, 1) ~= length(theta)
	theta = theta';
end
t = (0:N-1) * T;


clf

%time plot
% subplot(3,1,1)
% plot(t, theta);
% xlabel('time (secs)');
% ylabel('theta (deg)');

% acf plot
%subplot(3,1,2)
unsmooth_acf = xcorr(theta-mean(theta), 'unbiased');


acf = smooth(unsmooth_acf, n_smooth);


unsmooth_acf = unsmooth_acf(N:end);
plot(unsmooth_acf)
acf = acf(N:end);

lag = 0:N-1;
% plot(lag * T, acf, '.-')
% xlabel('lag (seconds)');
% ylabel('ACF');
% hold on

% now, we want to find the primary freq, which is the first
% maximum in the acf away from zero lag
optima = [];
dacf = diff(acf);
for l = 2:N-1
	if dacf(l) == 0
		dacf(l) = 1e-8;
	end

	if (dacf(l-1)*dacf(l)) < 0
		if dacf(l) > 0
			optima = [optima [-1; l]];
		else
			optima = [optima [1; l]];
		end
	end
end

if size(optima)==[0,0]
    disp('no optima');

    f=NaN; return      
end

% plot optima
type = optima(1, :);
lagi = optima(2, :);
%plot(lag(lagi) * T, acf(lagi), 'ro')

if length(type)<3
    disp('optima invalid e1');

    f=NaN; return      
end

% confirm optima types are what we expect
%if any(type(1:3) ~= [-1 1 -1])
%	disp('optima invalid e2');
%     pause
 %   f=NaN; return      
%end

% confirm timings are what we expect
T_whisk = lagi(2);
T_pred = T_whisk * [0.5 1 1.5];
%perf = lagi(1:3) ./ T_pred;
%if any(perf > 2.0) || any(perf < 0.3)
%	disp('optima invalid e3');

%    f=NaN; return
%end

% we now have a good estimate of whisk freq
% optimise wrt original (unsmoothed) acf
% subplot(3,1,3)
% plot(lag * T, unsmooth_acf, '.-');
% hold on

% optimise
T_whisk
while unsmooth_acf(T_whisk+1) > unsmooth_acf(T_whisk)
	T_whisk = T_whisk + 1;
end
while unsmooth_acf(T_whisk-1) > unsmooth_acf(T_whisk)
	T_whisk = T_whisk - 1;
end

%plot(lag(T_whisk) * T, unsmooth_acf(T_whisk), 'ro')

% return freq to caller
f = 1 / (lag(T_whisk) * T);


