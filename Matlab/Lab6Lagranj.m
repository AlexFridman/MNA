syms x X y l L
X = 0:0.1:1;
y = [1; 1.41; 1.79; 2.13; 2.46; 2.76; 3.04; 3.3; 3.55; 3.79; 4.01];

for i=1:11
    l(i)=1;
    for j=1:10
        if (i ~= j)
            l(i) = l(i) * (x - X(j)) / (X(i) - X(j));
        end;
    end;
end;

L = x*0;
for i=1:11
    L = L + l(i) * y(i);
end;

pretty(simplify(L))
vpa(subs(L,0.47),5)
