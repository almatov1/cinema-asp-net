CREATE TABLE sessions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    movie_title TEXT NOT NULL,
    date_time TIMESTAMPTZ NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE bookings (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    session_id UUID NOT NULL,
    seat_number INT NOT NULL,
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    CONSTRAINT fk_bookings_sessions FOREIGN KEY (session_id) REFERENCES sessions(id),
    CONSTRAINT uq_session_seat UNIQUE (session_id, seat_number),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
